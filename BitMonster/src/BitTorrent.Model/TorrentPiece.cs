using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace BitTorrent.Model
{
   [Serializable]
   public class TorrentPiece
   {
      public uint Index { get; set; }

      public TorrentPieceChunk[] Chunks { get; private set; }
      public byte[] Hash { get; private set; }

      public TorrentPeer AssignedPeer { get; set; }

      public byte[] Data
      {
         get { return _data; }
         set
         {
            if (value == null)
            {
               _indexesReceived.Clear();
               _bytesWritten = 0;
               var ip = string.Empty;
               if (AssignedPeer != null)
               {
                  ip = AssignedPeer.IpAddress;
               }
               Logger.Info(string.Format("TorrentPiece.Data is set to null by: {0} Thread: {1}", ip, Thread.CurrentThread.ManagedThreadId), this.Classname(), "Data");
            }
            _data = value;
         }
      }

      public uint Size { get; private set; }

      public long FilePosition { get; private set; }

      [NonSerialized]
      private uint _bytesWritten;
      [NonSerialized]
      private byte[] _data;
      
      private object _WriteFileLock = new object();
      private object _AddBytesLock = new object();
      private IList<int> _indexesReceived;

      public TorrentPiece()
      {
         _indexesReceived = new List<int>();
      }

      public static IList<TorrentPiece> GenerateTorrentPieces(long totalFilesSize, uint pieceSize, string[] hashes)
      {
         var ret = new List<TorrentPiece>();
         uint chunkSize;

         if (pieceSize % Torrent.DataReqLength == 0)
         {
            chunkSize = Torrent.DataReqLength;
         }
         else
         {
            chunkSize = pieceSize / 2;
         }

         uint numOfChunks = pieceSize / chunkSize;
         long bytesUsed = 0;

         for (uint i = 0; i < hashes.Length; i++)
         {
            var piece = new TorrentPiece
            {
               Hash = Utils.GetBytes(hashes[i]),
//               Chunks = new TorrentPieceChunk[numOfChunks],
               Index = i
            };

            var chunks = new List<TorrentPieceChunk>((int)numOfChunks);
            uint pSize = 0;
            for (uint j = 0; j < numOfChunks; j++)
            {
               long bytesTotalLeft = totalFilesSize - bytesUsed;

               if (bytesTotalLeft > 0)
               {
                  var chunk = new TorrentPieceChunk {Offset = j * chunkSize};
//                  piece.Chunks[j].Offset = (uint) (j*chunkSize);

                  if (bytesTotalLeft < chunkSize)
                  {
//                     piece.Chunks[j].Amount = (uint) bytesTotalLeft;
                     chunk.Amount = (uint) bytesTotalLeft;
                  }
                  else
                  {
//                     piece.Chunks[j].Amount = chunkSize;
                     chunk.Amount = chunkSize;
                  }

                  pSize += chunk.Amount;
                  bytesUsed += chunk.Amount;
                  chunks.Add(chunk);
               }
               piece.Chunks = chunks.ToArray();
            }

            piece.Size = pSize;
            piece.FilePosition = bytesUsed - pSize;
            ret.Add(piece);
         }

         return ret;
      }

      public bool Add(byte[] data, uint offset)
      {
         lock (_AddBytesLock)
         {
            int totPiecies = (int) Size/Torrent.DataReqLength;
            int pos = offset == 0 ? 1 : (int) offset/Torrent.DataReqLength + 1;

            Debug.WriteLine("******* Received piece Index: {0} {1}/{2}", Index, pos, totPiecies);

            if (!_indexesReceived.Contains(pos))
            {
               _indexesReceived.Add(pos);
               try
               {
                  if (Data == null)
                  {
                     Data = new byte[Size];
                  }

                  if (Data != null)
                  {
                     Array.Copy(data, 0, Data, offset, data.Length);
                     Logger.Info(
                        string.Format(
                           "TorrentPiece.Data Ip: {0} Thread: {1}. Received piece Index: {2} {3}/{4} Length: {5} Tot: {6}",
                           AssignedPeer.IpAddress, Thread.CurrentThread.ManagedThreadId, Index, pos, totPiecies,
                           data.Length, _bytesWritten + (uint) data.Length), this.Classname(), "Add");
                  }
                  else
                  {
                     Logger.Info(
                        string.Format(
                           "TorrentPiece.Data is NULL. Ip: {0} Thread: {1}. Received piece Index: {2} {3}/{4} Length: {5} Tot: {6}",
                           AssignedPeer.IpAddress, Thread.CurrentThread.ManagedThreadId, Index, pos, totPiecies,
                           data.Length, _bytesWritten + (uint) data.Length), this.Classname(), "Add");
                  }

                  _bytesWritten += (uint) data.Length;

                  if (_bytesWritten == Size)
                  {
                     var validated = Validate();
                     return validated;
                  }
                  else if (_bytesWritten > Size)
                  {
                     Data = null;
                  }
               }
               catch (Exception ex)
               {
                  Logger.Error(ex);
                  _bytesWritten = 0;
                  _indexesReceived.Clear();
                  Data = null;
               }
            }
            else
            {
               Logger.Info(
                        string.Format(
                           "TorrentPiece.Data Repeat Piece tried to be added!. Ip: {0} Thread: {1}. Received piece Index: {2} {3}/{4} Length: {5} Tot: {6}",
                           AssignedPeer.IpAddress, Thread.CurrentThread.ManagedThreadId, Index, pos, totPiecies,
                           data.Length, _bytesWritten + (uint)data.Length), this.Classname(), "Add");
            }
         }
         return false;
      }

      public void Write(IList<MyChunkFile> chunks)
      {
//         Debug.WriteLine("--------- Writing. Index: {0}, At: {1}, Length: {2}", Index, FilePosition, Data.Length);

         int bytesWritten = 0;
         foreach (var chunk in chunks)
         {
            WriteToFile(chunk.Filename, bytesWritten, chunk.FileOffset, chunk.Amount);
            bytesWritten += chunk.Amount;
            chunk.FileLink.BytesDownloaded += chunk.Amount;
         }
//         Data = null;
      }

      private void WriteToFile(string file, int start, long offset, int amount)
      {
         bool success = false;
         var finalFile = Path.Combine(TorrentApplication.SaveDir, file);
         do
         {
            try
            {
               lock (_WriteFileLock)
               {
                  var di = new FileInfo(finalFile).Directory;
                  if (di != null && !di.Exists)
                  {
                     di.Create();
                  }

                  using (var fs = new FileStream(finalFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                  {
                     fs.Seek(offset, SeekOrigin.Begin);
                     fs.Write(Data, start, amount);
                  }
                  success = true; 
               }
            }
            catch (IOException ex)
            {
               Debug.WriteLine("***error in WriteToFile(): " + ex);
               Thread.Sleep(5000);
               Logger.Error(ex);
            }  
         } while (!success);
      }

      public bool Validate()
      {
         var pieceHash = new SHA1CryptoServiceProvider().ComputeHash(Data);
         return pieceHash.SequenceEqual(Hash);
      }
   }

   [Serializable]
   public struct TorrentPieceChunk
   {
      public uint Offset { get; set; }
      public uint Amount { get; set; }
   }
}
