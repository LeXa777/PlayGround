using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace BitTorrent.Model
{
   [Serializable]
   public class TorrentFile
   {
      private long _length = -1;
      public long Length { get { return _length; } set { _length = value; } }
      public string Path { get; set; }
      public long GlobalOffset { get; set; }
      public long BytesDownloaded { get; set; }
      public bool Downloaded
      {
         get { return BytesDownloaded == Length; }
      }

      public override string ToString()
      {
         return string.Format("Length: [{0}] Path: [{1}]", Length, Path);
      }

      public static long FilesTotalLength(IList<TorrentFile> files)
      {
         long ret = 0;

         foreach (var file in files)
         {
            ret += file.Length;
         }

         return ret;
      }

      public static IList<MyChunkFile> GetFiles(IList<TorrentFile> files, long start, long amount)
      {
         #region Check Arguments
         if (start < 0 || start + amount > FilesTotalLength(files))
         {
            throw new ArgumentOutOfRangeException(
               string.Format("Start pos given: [{0}], Minimum 0. Total amount given: {1}, Max expected: {2}",
               start, amount, start + amount));
         }
         # endregion
         var ret = new List<MyChunkFile>();
         long amountLeft = amount;
         long currentPos = start;

         foreach (var file in files)
         {
            long nextFileStart = file.GlobalOffset + file.Length;

            if (start < nextFileStart)
            {
               long fileOffset = currentPos - file.GlobalOffset;

               int fileAmount;
               long globalPosOffset = file.GlobalOffset + fileOffset + amountLeft;
               if (globalPosOffset <= nextFileStart)
               {
                  fileAmount = (int)amountLeft;
               }
               else
               {
                  fileAmount = (int)(file.Length - fileOffset);
               }

               amountLeft -= fileAmount;
               currentPos += fileAmount;

               ret.Add(new MyChunkFile { Filename = file.Path, FileOffset = fileOffset, Amount = fileAmount, FileLink = file});
            }

            if (amountLeft <= 0)
            {
               if (amountLeft < 0)
               {
                  throw new Exception(
                     string.Format("Amount left less then 0: {0}, Requested start: {1}, amount: {2}", 
                     amountLeft, start, amount));
               }
               break;
            }
         }
         return ret;
      }
   }

   public class MyChunkFile
   {
      public string Filename { get; set; }
      public long FileOffset { get; set; }
      public int Amount { get; set; }
      public TorrentFile FileLink { get; set; }
   }
}
