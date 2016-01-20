using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using BitTorrent.Model.Bencode;

namespace BitTorrent.Model
{
   public delegate void TorrentUpdatedHandler(TorrentEvent peerEvent);
   public delegate void TorrentDownloadedHandler(TorrentEvent peerEvent);
   public delegate void TorrentChangedHandler(TorrentPeer peer);

   [Serializable]
   public class TorrentEvent : EventArgs
   {
      public Torrent Torrent { get; set; }
   }

   public enum TrackerRequestEvent
   {
      Started,
      Stopped,
      Completed,
      Regular //Not part of a protocol
   }

   public enum TorrentStatus
   {
      Added,
      Downloading,
      Paused,
      Stopped
   }

   [Serializable]
   public class AnnounceResponse
   {
      private int _interval = 240;
      private int _minInterval = 240;

      public int Interval
      {
         get { return _interval; }
         set { _interval = value; }
      }

      public int MinInterval {
         get
         {
            if (_minInterval == 0)
            {
               return _interval;
            }
            return _minInterval;
         }
         set { _minInterval = value; } 
      }

      public TorrentPeer[] Peers { get; set; }
      public string FailureMsg { get; set; }
      public string WarningMsg { get; set; }
      public string TrackerId { get; set; }
      public int Complete { get; set; }
      public int Incomplete { get; set; }
      public string FailureReason { get; set; }
   }

   [Serializable]
   public class Torrent
   {
      # region Events

      [field: NonSerialized]
      public event TorrentUpdatedHandler Updated;

      [field: NonSerialized]
      public event TorrentDownloadedHandler Downloaded;

      [field: NonSerialized]
      public event TorrentChangedHandler PeerAdded;

      [field: NonSerialized]
      public event TorrentChangedHandler PeerRemoved;

      # endregion

      # region Constants & Public Variables

      public const string ProtocolName = "BitTorrent protocol";
      public const string ClientName = "alexalexalexalexaley";
      public static byte[] ClientExtension = { 0, 0, 0, 0, 0, 0, 0, 0 };
      public const int DataReqLength = 0x4000;

      public TorrentStatus Status;

      public string TorrentName { get; private set; }
      public string TorrentFilename { get; private set; }

      public IList<Uri> Announce
      {
         get { return _announce; }
         private set { _announce = value; }
      }

      //      public IList<string> AnnounceList { get; private set; }
      public string Comment { get; private set; }
      public string CreatedBy { get; private set; }
      public DateTime AddedDate { get; private set; }
      public DateTime CreatedDate { get; private set; }
      public string Encoding { get; private set; }
      public string Publisher { get; private set; }
      public string PublisherUrl { get; private set; }
      public IList<TorrentPiece> Pieces { get; set; }
      public long FileDuration { get; private set; }
      public int FileMedia { get; private set; }
      public long FilePieceLength { get; private set; }
      public string[] FilePieceHashes { get; set; }
      public long TotalDownloadedBytes { get; private set; }
      public bool[] Bitfield { get; private set; }
      public long DownloadedBytes { get; private set; }
      public long UploadedBytes { get; private set; }

      public int DownSpeed
      {
         get
         {
            int tot = 0;

            if (Peers != null && Peers.Count > 0)
            {
               foreach (var peer in Peers)
               {
                  if (peer.Status == TorrentPeerStatus.HandshakeOk)
                  {
                     tot += peer.DownSpeed;
                  }
               }

               if (tot > 0)
               {
                  return tot / Peers.Count;
               }
            }

            return 0;
         }
      }

      public int UpSpeed { get; set; }
      public long CorruptBytes { get; private set; }
      public long RedunantBytes { get; private set; }
      public bool CompactMode { get; private set; }

      public string BifieldText
      {
         get
         {
            return Utils.GetBitfieldString(Bitfield);
         }
      }

      public Image BitfieldImage
      {
         get
         {
            return Utils.GetBitfieldImage(Bitfield);
         }
      }
    
      public Dictionary<string, string> FileProfiles { get; private set; }

      public IList<TorrentFile> Files { get; set; }

      // Additional

      public IList<TorrentPeer> Peers { get; set; }
      public int Progress { get; private set; }

      public byte[] InfoHashPlain
      {
         get { return _infoHashPlain; }
      }

      public int ActivePeers
      {
         get
         {
            int ret = 0;

            foreach (var peer in Peers)
            {
               if (peer.Status == TorrentPeerStatus.HandshakeOk)
               {
                  ret++;
               }
            }

            return ret;
         }
      }

      # endregion

      # region Private Variables

      private IList<Uri> _announce = new List<Uri>(); 
      private IList<AnnounceResponse> _announceResponses;
      private string _infoHash = string.Empty;
      private byte[] _infoHashPlain;
      private string _peerId = "olexalexalexalexaley";
      private string _peerHost = string.Empty;
      private int _peerPort;
      private FileInfo _diskFile;
      private object _fileLock = new object();
      private object _GetPiecesLock = new object();
      private DateTime _lastPeerCheckTime;
      private DateTime _lastRegRequestMade;

      # endregion

      # region Constructors

      private Torrent()
      {
         Peers = new List<TorrentPeer>();
         Files = new List<TorrentFile>();
         CompactMode = true;
      }

      private void CheckIfPeersBecameAvailable()
      {
         while (TorrentApplication.Running)
         {
            if (_lastPeerCheckTime.AddSeconds(30) < DateTime.Now)
            {
               for (int i = Peers.Count - 1; i >= 0; i--)
               {
                  var peer = Peers[i];
                  if (peer.Status != TorrentPeerStatus.HandshakeOk &&
                     peer.FailedConnectTime.AddMinutes(5) < DateTime.Now && peer.FailedConnectTries < 10)
                  {
                     Task.Factory.StartNew(peer.Connect);
                  }
               }
               _lastPeerCheckTime = DateTime.Now;
            }
            Thread.Sleep(1000);
         }
      }

      public Torrent(string filePath) : this()
      {
         AddedDate = DateTime.Now;
         Parse(filePath);
         Bitfield = new bool[FilePieceHashes.Length];
      }

      # endregion

      # region Public Methodsre

      public void Download()
      {
         Task.Factory.StartNew(GetPeersFromAnounce);
      }

      public IList<TorrentPiece> GetPieces(TorrentPeer peer)
      {
         // in feature need to implement rarest first... etc..

         var ret = new List<TorrentPiece>();
         lock (_GetPiecesLock)
         {
            for (int i = 0; i < Bitfield.Length; i++)
            {
               int maxPieces = Pieces.Count > 500 ? 30 : 15;

               if (ret.Count >= maxPieces)
               {
                  break;
               }

               if (!Bitfield[i] && peer.Bitfield[i]) // if our piece is missing and exist in caller peer
               {
                  var piece = Pieces[i];
                  
                  bool taken = piece.AssignedPeer != null &&
                     piece.AssignedPeer.Status == TorrentPeerStatus.HandshakeOk && 
                     piece.AssignedPeer.LasteReceivedPiece > DateTime.Now.AddMinutes(1);

                  bool assignedToMe = piece.AssignedPeer != null && piece.AssignedPeer == peer;

                  if (taken || assignedToMe)
                  {
                     continue;
                  }

                  piece.AssignedPeer = peer;
                  ret.Add(piece);
               }
            }
         }
         var sb = new StringBuilder(".... GetPieces(): ");

         foreach (var piece in ret)
         {
            sb.AppendFormat(" {0}", piece.Index);
         }

         Debug.WriteLine(".... GetPieces(): ");
         return ret;
         //return Pieces.ToArray();
      }

      public void AddPeers(IEnumerable<TorrentPeer> peers)
      {
         foreach (var peer in peers)
         {
            AddPeer(peer.IpAddress, peer.Port);
         }
      }

      public void AddPeer(string address, int port)
      {
         var peer = new TorrentPeer(InfoHashPlain, address, port);

         if (!Peers.Contains(peer))
         {
            peer.PieceReceived += peer_PieceReceived;
            peer.Connected += peer_Connected;
            peer.Disconnected += peer_Disconnected;
            Peers.Insert(0, peer);
            Task.Factory.StartNew(peer.Connect);
         }
      }

      public byte[] GetPieceFromDisk(uint pIndex, uint pOffset, uint amount)
      {
         long filePos = FilePieceLength * pIndex + pOffset;
         var buffer = new byte[amount];

         lock (_fileLock)
         {
            var files = TorrentFile.GetFiles(Files, filePos, (int)amount);

            int bytesReadSoFar = 0;
            foreach (var file in files)
            {
               var finalFile = Path.Combine(TorrentApplication.SaveDir, file.Filename);

               if (!File.Exists(finalFile))
               {
                  throw new FileNotFoundException(finalFile);
               }

               using (var fs = new FileStream(finalFile, FileMode.Open, FileAccess.Read))
               {
                  fs.Seek(file.FileOffset, SeekOrigin.Begin);
                  fs.Read(buffer, bytesReadSoFar, file.Amount);
                  bytesReadSoFar += file.Amount;
               }
            }
         }

         return buffer;
      }

      public void DeletePeer(TorrentPeer peer)
      {
         peer.Connected -= peer_Connected;
         peer.Disconnected -= peer_Disconnected;
         peer.PieceReceived -= peer_PieceReceived;
         peer.Disconnect();

         Peers.Remove(peer);
      }

      public void WakeUp()
      {
         MakeRegularTrackerRequests();
         Task.Factory.StartNew(CheckIfPeersBecameAvailable);
//         foreach (var peer in Peers)
//         {
//            peer.Connect();
//         }
      }

      # endregion

      # region Event Handlers

      void peer_Connected(TorrentPeer peer)
      {
         //peer.Download();
         if (PeerAdded != null)
         {
            PeerAdded(peer);
         }
      }

      void peer_Disconnected(TorrentPeer peer)
      {
         if (PeerRemoved != null)
         {
            PeerRemoved(peer);
         }
      }

      private void peer_PieceReceived(TorrentPiece piece)
      {
         lock (this)
         {
            if (!Bitfield[piece.Index])
            {
               TotalDownloadedBytes += piece.Data.Length;
               long onePercent = TorrentFile.FilesTotalLength(Files) / 100;
               Progress = (int)(TotalDownloadedBytes / onePercent);
               Bitfield[piece.Index] = true;
               DownloadedBytes += piece.Size;

               if (Updated != null)
               {
                  Updated(new TorrentEvent { Torrent = this });
               }

               WriteToFile(TorrentFile.GetFiles(Files, piece.FilePosition, (int)piece.Size), piece);

               lock (piece)
               {
                  var stack = new StackTrace(false);
                  Logger.Info(string.Format("About to set TorrentPiece.Data to null. Ip: {0} Thread: {1} Length: {2} of {3} Stack: {4}", 
                     piece.AssignedPeer.IpAddress, Thread.CurrentThread.ManagedThreadId, piece.Data.Length, piece.Size, stack), this.Classname(), "peer_PieceReceived");
                  piece.Data = null;
               }

               foreach (var peer in Peers.ToArray())
               {
                  if (peer.Status == TorrentPeerStatus.HandshakeOk)
                  {
                     peer.SendHave(piece);
                  }
               }

               //TODO: More than one thread can endup here one after another causing two Downloaded events...
               if (PiecesLeftToDownload() == 0)
               {
                  if (Downloaded != null)
                  {
                     Downloaded(new TorrentEvent { Torrent = this });
                  }

                  var responses = AnnounRequest(TrackerRequestEvent.Completed);
               }
            } 
         }
      }

      # endregion

      # region Private Helpers

      private void GetPeersFromAnounce()
      {
         var responses = AnnounRequest(TrackerRequestEvent.Started);
         _announceResponses = responses;

         foreach (var response in responses)
         {

            if (!string.IsNullOrEmpty(response.FailureMsg))
            {
               throw new TorrentException(this, "An error in a tracker response. Error: " + response.FailureMsg);
            }

            if (!string.IsNullOrEmpty(response.WarningMsg))
            {
               Logger.Info(string.Format("{0} {1}", response.WarningMsg, TorrentFilename), "GetPeersFromAnounce", GetType().ToString());
            }

            MakeRegularTrackerRequests();

            if (response.Peers != null && response.Peers.Length > 1)
            {
               AddPeers(response.Peers);
            }
         }
      }

      private void MakeRegularTrackerRequests()
      {
         if (_announceResponses != null && _announceResponses.Count > 0)
         {
            foreach (var response in _announceResponses)
            {
               new Thread(MakeRegularTrackerRequestsThread).Start(response);
            }
         }
      }

      private void MakeRegularTrackerRequestsThread(object res)
      {
         var response = (AnnounceResponse) res;
         while (TorrentApplication.Running)
         {
            if (_lastRegRequestMade.AddSeconds(response.MinInterval) < DateTime.Now)
            {
               var responses = AnnounRequest(TrackerRequestEvent.Regular);

               foreach (var r in responses)
               {
                  if (r.Peers != null)
                  {
                     AddPeers(r.Peers);
                  }
               }
               _lastRegRequestMade = DateTime.Now;
            }
            Thread.Sleep(1000);
         }
      }

      private int PiecesLeftToDownload()
      {
         var ret = 0;

         foreach (var b in Bitfield)
         {
            if (!b)
            {
               ret++;
            }
         }

         return ret;
      }

      private void WriteToFile(IList<MyChunkFile> files, TorrentPiece piece)
      {
         lock (_fileLock)
         {
            piece.Write(files);
         }
      }

      private IList<AnnounceResponse> AnnounRequest(TrackerRequestEvent requestEvent)
      {
         var ret = new List<AnnounceResponse>();
         var wClient = new WebClient();

         foreach (var url in Announce)
         {
            if (url.Scheme == Uri.UriSchemeHttp)
            {
               var q = new StringBuilder(url.AbsoluteUri);
               q.AppendFormat("{0}info_hash={1}", url.AbsoluteUri.Contains("?") ? "&" : "?", _infoHash);
               q.AppendFormat("&peer_id={0}", _peerId);
               q.AppendFormat("&port={0}", TorrentApplication.Port);
               q.AppendFormat("&uploaded={0}", UploadedBytes);
               q.AppendFormat("&downloaded={0}", DownloadedBytes);
               q.AppendFormat("&left={0}", TorrentFile.FilesTotalLength(Files) - DownloadedBytes);
               q.AppendFormat("&corrupt={0}", CorruptBytes);
               q.AppendFormat("&redundant={0}", RedunantBytes);
               q.Append("&compact=1");
               q.Append("&numwant=200");
               q.Append("&key=43947fcf");
               q.Append("&no_peer_id=1");
               q.Append("&compact=1");
               q.Append("&supportcrypto=0");

               if (requestEvent != TrackerRequestEvent.Regular)
               {
                  q.AppendFormat("&event={0}", requestEvent.ToString().ToLower());
               }

//               var uri = new Uri(q.ToString());
               wClient.Headers.Add("User-Agent", "BitMonster");
               try
               {
                  string response = Utils.GetText(wClient.DownloadData(q.ToString()));
                  var res = ParseResponse(response);
                  ret.Add(res);
               }
               catch (WebException) { }
               catch (Exception ex)
               {
                  Logger.Error(ex);
               }
            }
         }

         return ret;
      }

      private void CreateInfoHash(IBencode info)
      {
         _infoHashPlain = Utils.CalculateTorrentInfoHash(info);
         _infoHash = HttpUtility.UrlEncode(_infoHashPlain);
      }

      # endregion

      # region Parse Functions

      private void Parse(string filePath)
      {
         var bc = Utils.Parse(new FileInfo(filePath)) as BencodeDictionary;

         if (bc == null)
         {
            throw new Exception("Could not open file: " + filePath);
         }

         TorrentFilename = new FileInfo(filePath).Name;

         foreach (var b in bc)
         {
            switch (b.Key)
            {
               case "announce":
                  Announce = new [] {new Uri(b.Value.ToString()) };
                  break;
               case "announce-list":
                  Announce = ParseAnnounceList(b.Value);
                  break;
               case "comment":
                  Comment = b.Value.ToString();
                  break;
               case "created by":
                  CreatedBy = b.Value.ToString();
                  break;
               case "creation date":
                  CreatedDate = Utils.UnixTimeStampToDateTime(double.Parse(b.Value.ToString() ));
                  break;
               case "encoding":
                  Encoding = b.Value.ToString();
                  break;
               case "publisher":
                  Publisher = b.Value.ToString();
                  break;
               case "publisher-url":
                  PublisherUrl = b.Value.ToString();
                  break;
               case "info":
                  CreateInfoHash(b.Value as BencodeDictionary);
                  ParseInfo(b.Value);
                  break;
               default:
                  Logger.Warning(string.Format("Not implemented key:[ {0}] Value: [{1}]", b.Key, b.Value), GetType().ToString(), "Parse");
                  break;
            }
         }
      }

      private void ParseInfo(IBencode bencode)
      {
         var lonelyFile = new TorrentFile();

         if (bencode is BencodeDictionary)
         {
            var dic = bencode as BencodeDictionary;

            foreach (KeyValuePair<string, IBencode> b in dic)
            {
               try
               {
                  switch (b.Key)
                  {
                     case "file-duration":
                        FileDuration = long.Parse(Utils.TrimString(b.Value.ToString()));
                        break;
                     case "file-media":
                        int fileMedia;
                        if (int.TryParse(b.Value.ToString(), out fileMedia))
                        {
                           FileMedia = fileMedia;
                        }
                        break;
                     case "length":
                        lonelyFile.Length = long.Parse(Utils.TrimString(b.Value.ToString()));
                        break;
                     case "name":
                        lonelyFile.Path = Utils.GetUtf8String(b.Value.ToString());
                        break;
                     case "piece length":
                        FilePieceLength = long.Parse(Utils.TrimString(b.Value.ToString()));
                        break;
                     case "pieces":
                        ParsePiecesHashes(b.Value.ToString());
                        break;
                     case "profiles":
                        ParseProfiles(b.Value);
                        break;
                     case "files":
                        ParseFiles(b.Value);
                        break;
                     default:
                        Logger.Warning(string.Format("Not implemented key:[ {0}] Value: [{1}]", b.Key, b.Value), GetType().ToString(), "ParseInfo");
                        break;
                  }
               }
               catch (Exception ex)
               {
                  Logger.Error(ex);
               }
            }
         }

         if (!string.IsNullOrEmpty(lonelyFile.Path))
         {
            TorrentName = lonelyFile.Path.Replace('_', ' ');
            if (lonelyFile.Length == -1)
            {
               for (int i = 0; i < Files.Count; i++)
               {
                  Files[i].Path = lonelyFile.Path + "/" + Files[i].Path;
               }
            }
            else
            {
               Files.Add(lonelyFile);
            }
         }
         
         Pieces = TorrentPiece.GenerateTorrentPieces(TorrentFile.FilesTotalLength(Files), (uint)FilePieceLength, FilePieceHashes);
      }

      private void ParsePiecesHashes(string hashes)
      {
         FilePieceHashes = new string[hashes.Length / 20];

         for (int i = 0, j = 0; i < hashes.Length; i += 20, j++)
         {
            FilePieceHashes[j] = hashes.Substring(i, 20);
         }
      }

      private void ParseFiles(IBencode bencode)
      {
         var list = bencode as List<IBencode>;

         if (list != null)
         {
            long globalOffset = 0;
            foreach (var l in list)
            {
               var dic = l as Dictionary<string, IBencode>;

               if (dic != null && dic.Count == 2 )
               {
                  var length = long.Parse(dic["length"].ToString());
                  var path = dic["path"];
                  var pathStr = string.Empty;

                  var pathList = path as List<IBencode>;

                  for (int i = 0; i < pathList.Count; i++ )
                  {
                     pathStr += Utils.GetUtf8String(pathList[i].ToString());
                     pathStr += (i < pathList.Count - 1) ? "/" : string.Empty;
                  }

                  Files.Add(new TorrentFile { Length = length, Path = pathStr, GlobalOffset = globalOffset });

                  globalOffset += length;
               }
            }

            if (Files.Count > 0)
            {
               TorrentName = Files[0].Path.Replace('_', ' ');
            }
         }
      }

      private IList<Uri> ParseAnnounceList(IBencode bencode)
      {
         var ret = new List<Uri>();
         if (bencode is BencodeList)
         {
            var bList = bencode as IList<IBencode>;
            foreach(var b in bList)
            {
               ret.Add(new Uri(b.ToString().Trim('\n', '\r')));
            }
         }

         return ret;
      }

      private void ParseProfiles(IBencode list)
      {
         if (list is BencodeList)
         {
            var enumList = list as List<IBencode>;
            foreach (var innerList in enumList)
            {
               if (innerList is BencodeDictionary)
               {
                  FileProfiles = new Dictionary<string, string>();
                  var iList = innerList as Dictionary<string, IBencode>;

                  foreach (var l in iList)
                  {
                     FileProfiles.Add(l.Key, l.Value.ToString());
                  }
               }
            }
         }
      }

      private AnnounceResponse ParseResponse(string strResponse)
      {
         var ret = new AnnounceResponse();
         try
         {
            var bDic = Utils.Parse(strResponse) as Dictionary<string, IBencode>;

            foreach (KeyValuePair<string, IBencode> b in bDic)
            {
               switch (b.Key)
               {
                  case "interval":
                     ret.Interval = int.Parse(b.Value.ToString());
                     break;
                  case "min interval":
                     ret.MinInterval = int.Parse(b.Value.ToString());
                     break;
                  case "peers":
                     ret.Peers = ParsePeers(b.Value.ToString());
                     break;
                  case "failure message":
                     ret.FailureMsg = b.Value.ToString();
                     break;
                  case "warning message":
                     ret.WarningMsg = b.Value.ToString();
                     break;
                  case "failure reason":
                     ret.FailureReason = b.Value.ToString();
                     break;
                  case "tracker id":
                     ret.TrackerId = b.Value.ToString();
                     break;
                  case "complete":
                     ret.Complete = int.Parse(b.Value.ToString());
                     break;
                  case "incomplete":
                     ret.Incomplete = int.Parse(b.Value.ToString());
                     break;
                  default:
                     Logger.Warning(string.Format("Not implemented key:[ {0}] Value: [{1}]", b.Key, b.Value), GetType().ToString(), "ParseResponse");
                     break;
               }
            }
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
         }

         return ret;
      }

      private TorrentPeer[] ParsePeers(string strPeers)
      {
         var peers = new List<TorrentPeer>(strPeers.Length / 6);

         for (int i = 0, j = 0; j < strPeers.Length; i++, j += 6)
         {
            try
            {
               string val = strPeers.Substring(j, 6);
               string ip = val.Substring(0, 4);
               string strPort = val.Substring(4, 2);

               byte[] ipB = Utils.GetBytes(ip);

               _peerHost = string.Format("{0}.{1}.{2}.{3}", (int)ipB[0], (int)ipB[1], (int)ipB[2], (int)ipB[3]);
               byte[] pBytes = Utils.GetBytes(strPort);
               var port = (UInt16) ((pBytes[0] << 8) | pBytes[1]);
               //var newPort = BencodingUtils.ByteArrToInt(pBytes);
               peers.Add(new TorrentPeer(InfoHashPlain, _peerHost, port));
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }
         }

         return peers.ToArray();
      }

      # endregion

      # region Object Overrides

      public override string ToString()
      {
         var sb = new StringBuilder();
         sb.AppendLine("Filename: " + TorrentFilename);
         sb.AppendLine("Announce: " + Announce);

//         if (AnnounceList != null)
//         {
//            sb.Append("Announce-List: [");
//
//            foreach (var an in AnnounceList)
//            {
//               sb.Append(string.Format(@"""{0}"" ", BencodingUtils.TrimString(an)));
//            }
//
//            sb.AppendLine("]");
//         }

         sb.AppendLine("Comment: " + Comment);
         sb.AppendLine("Created By: " + CreatedBy);

         sb.AppendLine("Creation Date: " + CreatedDate);
         sb.AppendLine("Encoding: " + Encoding);
         sb.AppendLine("Publisher: " + Publisher);
         sb.AppendLine("Publisher URL: " + PublisherUrl);

         //sb.AppendLine("TorrentName: " + TorrentName);
         sb.AppendLine("FileDuration: " + FileDuration);
         sb.AppendLine("FileMedia: " + FileMedia);
         //sb.AppendFormat("FileLength: {0:#,##0} bytes\n", FileLength);
         sb.AppendFormat("FilePieceLength: {0:#,##0} bytes\n", FilePieceLength);

         if (FileProfiles != null)
         {
            sb.AppendLine("Profiles: -----------------------");
            foreach (var p in FileProfiles)
            {
               sb.AppendFormat("\tKey [{0}] Value: [{1}]\n", p.Key, p.Value);
            }
         }

         if (Files != null)
         {
            sb.AppendFormat("\nFiles [{0}]:\n", Files.Count);

            foreach (var file in Files)
            {
               //sb.AppendLine("\n***************************");
               sb.AppendLine(file.ToString());
               //sb.AppendLine("***************************");
            }
         }
         return sb.ToString();
      }

      # endregion

      public void Stop()
      {
         Disconnect();
         this.Status = TorrentStatus.Stopped;
      }

      private void Disconnect()
      {
         foreach (var peer in Peers)
         {
            peer.Disconnect();
         }
      }
   }
}
