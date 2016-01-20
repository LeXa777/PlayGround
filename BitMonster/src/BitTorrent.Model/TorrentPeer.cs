using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
//using BitTorrent.BencodeUtils;

namespace BitTorrent.Model
{
   public delegate void PeerConnectedHandler(TorrentPeer peer);
   public delegate void HandshakeRecievedHandler(TorrentPeer peer);
   public delegate void HavePieceHandler(uint piece);
   public delegate void PieceHandler(TorrentPiece piece);
   public delegate void BitfieldRecievedHandler(TorrentPeer peer);

   [Serializable]
   public class TorrentPeer
   {
      # region Events

      public event PeerConnectedHandler Connected;
      public event PeerConnectedHandler Disconnected;
      public event HandshakeRecievedHandler HandshakeReceived;
      public event HavePieceHandler HavePiece;
      public event PieceHandler PieceReceived;
      public event BitfieldRecievedHandler BitfieldRecieved;

      # endregion

      # region Constants

      public const int HandshakeLength = 68;
      public const int MessageHeaderLength = 5;

      # endregion

      # region Private Members

      [NonSerialized] private Dictionary<uint, TorrentPiece> _pieces;
      
      [NonSerialized] private Socket _socket;

      private byte[] _piece; //TODO: Optimize later

      [NonSerialized] private Thread _receiveThread;

      [NonSerialized] private Thread _downloadThread;

      private readonly object _getSocketLock = new object();

      [NonSerialized] private DateTime _lasteReceived;

      [NonSerialized] private DateTime _lasteReceivedPiece;

      [NonSerialized] private bool _IsDisconnected;

      [NonSerialized] private bool _handshakeSent;

      [NonSerialized] private DateTime _failedConnectTime;

      [NonSerialized] private int _failedConnectTries;

      [NonSerialized] private long _totalReceived;

      [NonSerialized] private int _downSpeed;

      [NonSerialized] private int _upSpeed;

      [NonSerialized] IList<double> _downSpeedList;
      [NonSerialized] private bool _downloading;
      [NonSerialized] private bool _interestedSent;
      [NonSerialized] private int _chunksBuffer;
      [NonSerialized] private object _interestedLock = new object();

      public byte[] InfoHash { get; set; }

      # endregion

      # region Public Properties

      public string IpAddress { get; private set; }
      public int Port { get; private set; }
      public byte[] Id { get; set; }
      public byte[] Extensions { get; set; }
      public TorrentPeerStatus Status { get; set; }
      public bool[] Bitfield { get; set; }
      public bool MeChocked { get; set; }
      public bool HimChocked { get; set; }
      public int DownSpeed { get; private set; }
      public int UpSpeed { get; private set; }

      public DateTime LasteReceivedPiece 
      { 
         get { return _lasteReceivedPiece; } 
//         private set { _lasteReceivedPiece = value; } 
      }
      public DateTime FailedConnectTime { get { return _failedConnectTime; } set { _failedConnectTime = value; } }

      public int FailedConnectTries { get { return _failedConnectTries; } set { _failedConnectTries = value; } }

      private Torrent MyTorrent
      {
         get
         {
            var key = Utils.GetText(InfoHash);
            if (TorrentApplication.Torrents.ContainsKey(key))
            {
               return TorrentApplication.Torrents[key];
            }
            return null;
         }
      }

      private TorrentPeerStatus _status = TorrentPeerStatus.Uknown;

      # endregion

      # region Constractors

      public TorrentPeer(Socket socket)
      {
         _socket = socket;
         var ipEp = (IPEndPoint)socket.RemoteEndPoint;
         IpAddress = ipEp.Address.ToString();
         Port = ipEp.Port;
         _receiveThread = new Thread(new ThreadStart(ReceiveMsg));
         _receiveThread.Start();
         Initialize();
      }

      public TorrentPeer(byte[] infoHash, string ip, int port)
      {
         InfoHash = infoHash;
         IpAddress = ip;
         Port = port;
         Initialize();
      }

      private void Initialize()
      {
         this.Connected += TorrentConnected;
         this.Disconnected += TorrentPeer_Disconnected;
         this.HandshakeReceived += TorrentPeer_HandshakeReceived;
         this.HavePiece += TorrentPeer_HavePiece;
         this.BitfieldRecieved += TorrentPeer_BitfieldRecieved;
         MeChocked = true;
         HimChocked = true;
      }

      # endregion

      # region Event Handlers

      void TorrentConnected(TorrentPeer peer)
      {
         Debug.WriteLine(string.Format("***Peer connected [{0}]", IpAddress));
         if (!_IsDisconnected)
         {
            Handshake();
         }
      }

      void TorrentPeer_Disconnected(TorrentPeer peer)
      {
         if (Status != TorrentPeerStatus.HandshakeFailed)
         {
            Debug.WriteLine(string.Format("***Peer disonnected [{0}]", IpAddress));
         }
      }

      void TorrentPeer_HandshakeReceived(TorrentPeer peer)
      {
         Status = TorrentPeerStatus.HandshakeOk;

         if (!_handshakeSent)
         {
            Thread.Sleep(1000);
            Handshake();
            Thread.Sleep(200);
            SendBitfield();
         }

         if (Bitfield != null)
         {
            if (!_downloading)
            {
               _downloading = true;
               Download();
            }
         }

         Task.Factory.StartNew(() => //TODO: Refactor! I should be able to kill it... or stop it..
         {
            while (_socket != null)
            {
               Thread.Sleep(10 * 1000);
               if (_lasteReceived.AddSeconds(60) < DateTime.Now)
               {
                  try
                  {
                     GetSocket().Send(new byte[] { 0, 0, 0, 0 }); // keep alive
                  }
                  catch
                  {
                  }
               }
            }
         });
      }

      void TorrentPeer_BitfieldRecieved(TorrentPeer peer)
      {
         if (!_downloading)
         {
            _downloading = true;
            Download();
         }
      }

      void TorrentPeer_HavePiece(uint piece)
      {
         if (Bitfield.Length - 1 >= piece)
         {
            Bitfield[piece] = true;
         }
         else
         {
            Logger.Info(string.Format("Bitfield.Length Out of range: {0} piece: {1}", 
               Bitfield.Length, piece), "TorrentPeer_HavePiece", GetType().ToString());
         }
      }

      # endregion

      # region Public Methods

      public void Connect()
      {
         Debug.WriteLine(string.Format("***Peer about to try connecting [{0}]", IpAddress));
         GetSocket();
      }

      public string GetBitfieldImage()
      {
         return Utils.GetBitfieldString(Bitfield);
      }

      public void Download()
      {
         if (_downloadThread == null)
         {
            _pieces = new Dictionary<uint, TorrentPiece>();
            _downloadThread = new Thread(DownloadAsync);
            _downloadThread.Start();
         }
      }

      private void DownloadAsync()
      {
         while (_socket != null) // do better...
         {
            //TODO: Even before downloading check how many pieies there are... No point connecting if 0 pieces are available
            var pieces = MyTorrent.GetPieces(this);

            if (pieces.Count == 0)
            {
               Thread.Sleep(30*1000);
            }
            else
            {
               foreach (var piece in pieces)
               {
                  if (!_pieces.ContainsKey(piece.Index))
                  {
                     _pieces.Add(piece.Index, piece);
                  }

                  foreach (var chunk in piece.Chunks)
                  {
                     DownloadPiece(piece.Index, chunk.Offset, chunk.Amount);
                     Thread.Sleep(50);
                  }
               }
            }
         }
      }

      private void DownloadPiece(uint pIndex, uint offset, uint amount)
      {
         Debug.WriteLine("Begin Download pIndex: {0} offset: {1} amount: {2}", pIndex, amount, offset);
         if (_interestedLock == null)
            _interestedLock = new object();

         lock (_interestedLock)
         {
            if (MeChocked && !_interestedSent)
            {
               _interestedSent = true;
               SendInterested();
            } 
         }

         int count = 0;
         while (MeChocked && count < 50)
         {
            Thread.Sleep(100);
            count++;
         }

         if (!MeChocked)
         {
            while (_chunksBuffer > 10)
            {
               Thread.Sleep(1000); // one sec
            }

            _chunksBuffer++;
            SendRequestPiece(pIndex, offset, amount);
         }
      }

      # endregion

      # region Private Helpers

      private Socket GetSocket()
      {
         bool sendConnectedEvent = false;

         try
         {
            lock (_getSocketLock)
            {
               if (_socket == null || (_socket != null && !_socket.Connected))
               {
                  MeChocked = true;
                  Status = TorrentPeerStatus.Uknown;
                  _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                     ProtocolType.Tcp);
                  _socket.Connect(IpAddress, Port);
                  sendConnectedEvent = true;
               }

               if (_receiveThread == null || (_receiveThread != null && !_receiveThread.IsAlive))
               {
                  _receiveThread = new Thread(new ThreadStart(ReceiveMsg));
                  _receiveThread.Start();
               }

               if (sendConnectedEvent)
               {
                  if (Connected != null)
                  {
                     Connected(this);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Status = TorrentPeerStatus.HandshakeFailed;
            Disconnect();
            //Logger.Error(ex);
         }

         return _socket;
      }

      public void Disconnect()
      {
         FailedConnectTime = DateTime.Now;
         FailedConnectTries++;
         _downloading = _interestedSent = false;
         Status = TorrentPeerStatus.Uknown;
         try
         {
            if (_downloadThread != null)
            {
               try
               {
                  _downloadThread.Abort();
               }
               catch { }
            }

            if (_receiveThread != null)
            {
               try
               {
                  _receiveThread.Abort();
               }
               catch { }
               
            }

            if (_socket != null)
            {
               try
               {
                  _socket.Disconnect(false);
               }
               catch { }

               try
               {
                  _socket.Dispose();
               }
               catch { }
            }
         }
         catch
         {
         }
//         _socket = null;
         _receiveThread = null;
         MeChocked = true;
//         HimChocked = true;
         Bitfield = null;

         if (Disconnected != null)
         {
            Disconnected(this);
         }
      }

      private void Handshake()
      {
         Debug.WriteLine("Handshake()");
         try
         {
            List<byte> byteList = new List<byte>();
            byteList.Add((byte)Torrent.ProtocolName.Length);
            byteList.AddRange(Utils.GetBytes(Torrent.ProtocolName));
            byteList.AddRange(Torrent.ClientExtension);
            byteList.AddRange(InfoHash);
            byteList.AddRange(Utils.GetBytes(Torrent.ClientName));

            byte[] finalMsg = byteList.ToArray();

            _socket.Send(finalMsg);
            _handshakeSent = true;
         }
         catch
         {
            Status = TorrentPeerStatus.HandshakeFailed;
         }
//         return false;
      }

      private void ReceiveMsg()
      {
         var buffer = new byte[65552];
//         Debug.WriteLine("In ReceiveMsg(): Start [{0}]", IpAddress);

         while (true)
         {
            var queue = new RangeQueue<byte>();
            bool needMore = false;

            try
            {
               do
               {
//                  Debug.WriteLine("In ReceiveMsg(): Queue.Count: " + queue.Count);
                  if (queue.Count == 0 || needMore)
                  {
                     needMore = false;
//                     Debug.WriteLine("In ReceiveMsg(): About to receive stream");

                     int numOfBytesReceived = GetSocket().Receive(buffer);

                     _lasteReceived = DateTime.Now;
                     _totalReceived += numOfBytesReceived;

//                     Debug.WriteLine("In ReceiveMsg(): Received: " + numOfBytesReceived);
                     if (numOfBytesReceived == 0) // connection probably closed
                     {
//                        Debug.WriteLine("In ReceiveMsg(): Disconnecting");
//                        _socket = null;
                        this.Status = TorrentPeerStatus.Unaccessible;
                        Disconnect();
                        return;
                     }

                     byte[] bytesReceived = new byte[numOfBytesReceived];
                     Array.Copy(buffer, 0, bytesReceived, 0, numOfBytesReceived);
                     queue.AddRange(bytesReceived);
                  }

                  if (Status == TorrentPeerStatus.Uknown)
                  {
//                     Debug.WriteLine("In ReceiveMsg(): About to parse header");
                     int length = queue.DequeueRangePeek(1)[0];
                     if (queue.Count >= 68 && length == Torrent.ProtocolName.Length) // whole header received
                     {
                        ParseHandshake(queue.DequeueRange(68));
                     }
                     else
                     {
                        needMore = true;
//                        Debug.WriteLine("In ReceiveMsg(): Not enouph data to parse, will go back 1");
                     }
                  }
                  else
                  {
                     if (queue.Count >= 4)
                     {
                        bool keepAlive = false;
                        uint length = Utils.ByteArrToUInt(queue.DequeueRangePeek(4));

                        if (length > Torrent.DataReqLength * 1.5)
                        {
                           Debug.WriteLine("***error in ReceiveMsg(): Too much data received. Length: {0} queue.Count: {1}, Queue Data: {2}",
                              length, queue.Count, queue.DequeueRangePeek(queue.Count - 1));
                             queue.Clear();
                           continue;
                           //throw new Exception("Too much data received: " + length);
                        }

                        if (length != 0 && queue.Count != 4) // Only keep alive has a length of zero and is 4 byte long.
                        {
                           length--; // Decrise by one to get rid of message type
                        }
                        else
                        {
                           keepAlive = true;
                        }

                        if (keepAlive)
                        {
                           Debug.WriteLine("In ReceiveMsg(): Keep Alive received");
                           queue.DequeueRange(4);
                        }

                        if (queue.Count >= length + 5 && !keepAlive)
                        {
                           queue.DequeueRange(4);
                           var type = (TorrentMessageStatus)queue.Dequeue();

//                           Debug.WriteLine("In ReceiveMsg(): About to parse message. Length: " + length + " type: *** " +
//                                           type + " ***");
                           ParseMessage(type, queue.DequeueRange((int)length));
                        }
                        else
                        {
                           needMore = true;
//                           Debug.WriteLine("In ReceiveMsg(): Not enouph data to parse, will go back 2");
                        }
                     }
                     else
                     {
                        needMore = true;
//                        Debug.WriteLine("In ReceiveMsg(): Not enouph data to parse, will go back 3");
                     }
                  }
               } while (queue.Count > 0);
            }
            catch (ThreadAbortException) { }
            catch (SocketException) { }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }
         }
      }

      private void CalculateDownSpeed(int num)
      {
         if (num != 0 && LasteReceivedPiece > DateTime.Today)
         {
            var dt = DateTime.Now - LasteReceivedPiece;
            if (dt.Milliseconds > 0)
            {
               double seconds = (double)dt.Milliseconds / 1000;
               double speed = num / seconds;
               DownSpeed = (int)CalculateMeanSpeed(speed);
            }
         }
      }

      private double CalculateMeanSpeed(double speed)
      {
         if (_downSpeedList == null)
         {
            _downSpeedList = new List<double>(30);
         }
         else while (_downSpeedList.Count > 30)
         {
            _downSpeedList.RemoveAt(0);
         }

         _downSpeedList.Add(speed);

         var tot = 0.0;
         foreach (var i in _downSpeedList)
         {
            tot += i;
         }

         if (_downSpeedList.Count > 0 && tot > 0 )
         {
            return tot / _downSpeedList.Count;
         }
         return 0;
      }

      private void ParseHandshake(byte[] msg)
      {
         _downloading = false;
         int length = msg[0];
         string protocol = Utils.GetText(msg, 1, length);

         if (protocol != Torrent.ProtocolName)
         {
            throw new Exception("Uknown protocol found.");
         }

         int pos = 1;
         byte[] extension = new byte[8];
         byte[] infoHash = new byte[20];
         byte[] peerId = new byte[20];

         pos += length;
         Array.Copy(msg, pos, extension, 0, 8);
         pos += 8;
         Array.Copy(msg, pos, infoHash, 0, 20);
         pos += 20;
         Array.Copy(msg, pos, peerId, 0, 20);

         string infoHashStr = Utils.GetText(infoHash);
         if (!TorrentApplication.Torrents.ContainsKey(infoHashStr))
         {
            var info = string.Format("Info has do not match. Torrent: [{0}] My hash: [{1}] Received hash: [{2}]",
               MyTorrent.TorrentName, Utils.GetText(MyTorrent.InfoHashPlain),
               infoHashStr);
            Logger.Warning(info, GetType().ToString(), "ParseHandshake");
         }
         else
         {
            InfoHash = infoHash;
            Extensions = extension;
            Id = peerId;
            Status = TorrentPeerStatus.HandshakeOk;

            if (HandshakeReceived != null)
            {
               HandshakeReceived(this);
            }
         }
      }

      private void ParseMessage(TorrentMessageStatus msgType, byte[] msg)
      {
         try
         {
            uint pIndex;
            uint pOffset;

            switch (msgType)
            {
               case TorrentMessageStatus.Bitfield:
                  Bitfield = Utils.ParseBitfield(msg,
                     TorrentApplication.Torrents[Utils.GetText(InfoHash)].FilePieceHashes.Length);

                  if (BitfieldRecieved != null)
                  {
                     BitfieldRecieved(this);
                  }
                  break;

               case TorrentMessageStatus.Have:
                  uint pieceIndex = Utils.ByteArrToUInt(msg);
                  if (HavePiece != null)
                  {
                     HavePiece(pieceIndex);
                  }
                  break;

               case TorrentMessageStatus.Unchoke:
                  MeChocked = false;
                  break;

               case TorrentMessageStatus.Piece:
                  pIndex = Utils.ByteArrToUInt(msg, 0, 4);
                  pOffset = Utils.ByteArrToUInt(msg, 4, 4);

                  int length = msg.Length - 8;
                  CalculateDownSpeed(length);
                  _lasteReceivedPiece = DateTime.Now;
                  _piece = new byte[length];
                  Array.Copy(msg, 8, _piece, 0, length);
                  if (_pieces[pIndex].Add(_piece, pOffset))
                  {
                     Debug.WriteLine("-------- Piece Received: Peer: {0} Index: {1} / {2}, Offset: {3} ***", IpAddress, pIndex, MyTorrent.FilePieceHashes.Length-1, pOffset);
                     if (PieceReceived != null)
                     {
                        PieceReceived(_pieces[pIndex]);
                     }
                  }
                  _chunksBuffer--;
                  break;

               case TorrentMessageStatus.Interested:
                  HimChocked = false;
                  SendUnchoke();
                  break;

               case TorrentMessageStatus.Uninterested:
                  HimChocked = true;
                  break;

               case TorrentMessageStatus.Choke:
                  MeChocked = true;
                  Thread.Sleep(60000); // one minute
                  break;

               case TorrentMessageStatus.Extended:
                  // TODO: Implement extended message
                  break;

               case TorrentMessageStatus.Request:
                  if (!HimChocked)
                  {
                     pIndex = Utils.ByteArrToUInt(msg, 0, 4);
                     pOffset = Utils.ByteArrToUInt(msg, 4, 4);
                     uint amount = Utils.ByteArrToUInt(msg, 8, 4);
                     byte[] piece = MyTorrent.GetPieceFromDisk(pIndex, pOffset, amount);

                     if (piece != null && piece.Length == amount)
                     {
                        SendPiece(pIndex, pOffset, piece);
                     }
                     else
                     {
                        Debug.WriteLine(
                           "***error in ParseMessage() [case TorrentMessageStatus.Request]: could not get the right piece");
                     }
                  }
                  else
                  {
                     Debug.WriteLine(
                        "***error in ParseMessage() [case TorrentMessageStatus.Request]: Requesting data while being chocked");
                  }
                  break;

               default:
                  Logger.Warning(string.Format("Not implemented msg type: [{0}]", msgType), GetType().ToString(), "ParseMessage");
                  Disconnect(); 
                  break;
            }
         }
         catch (ThreadAbortException) { }
         catch (Exception ex)
         {
            Logger.Error(ex);
         }
      }

      private void SendInterested()
      {
         GetSocket().Send(new byte[] {0, 0, 0, 1, (int)TorrentMessageStatus.Interested});
      }

      private void SendRequestPiece(uint piece, uint start, uint end)
      {
         byte[] length = BitConverter.GetBytes(13).Reverse().ToArray();
         byte msgType = (byte) TorrentMessageStatus.Request;
         byte[] pieceIndex = BitConverter.GetBytes(piece).Reverse().ToArray();
         byte[] startFrom = BitConverter.GetBytes(start).Reverse().ToArray();
         byte[] pieceEnd = BitConverter.GetBytes(end).Reverse().ToArray();

         var byteList = new List<byte>();
         byteList.AddRange(length);
         byteList.Add(msgType);
         byteList.AddRange(pieceIndex);
         byteList.AddRange(startFrom);
         byteList.AddRange(pieceEnd);

         GetSocket().Send(byteList.ToArray());
      }

      private void SendBitfield()
      {
         byte[] bitfield = Utils.ParseBitFieldIntoBytes(MyTorrent.Bitfield);

         byte[] length = BitConverter.GetBytes(bitfield.Length + 1).Reverse().ToArray();
         var byteList = new List<byte>();
         byteList.AddRange(length);
         byteList.Add((byte)TorrentMessageStatus.Bitfield);

         byteList.AddRange(bitfield);

         GetSocket().Send(byteList.ToArray());
      }

      private void SendUnchoke()
      {
         var byteList = new List<byte>();
         byte[] length = BitConverter.GetBytes(1).Reverse().ToArray();
         byteList.AddRange(length);
         byteList.Add((byte) TorrentMessageStatus.Unchoke);
         byte[] finalMsg = byteList.ToArray();
         GetSocket().Send(finalMsg);
      }

      private void SendPiece(uint pIndex, uint pOffset, byte[] data)
      {
         byte[] length = BitConverter.GetBytes(data.Length + 9).Reverse().ToArray();

         byte[] bytePIndex = BitConverter.GetBytes(pIndex).Reverse().ToArray();
         byte[] bytePOffset = BitConverter.GetBytes(pOffset).Reverse().ToArray();

         var byteList = new List<byte>();
         byteList.AddRange(length);
         byteList.Add((byte)TorrentMessageStatus.Piece);

         byteList.AddRange(bytePIndex);
         byteList.AddRange(bytePOffset);

         byteList.AddRange(data);
         GetSocket().Send(byteList.ToArray());
      }

      public void SendHave(TorrentPiece piece)
      {
         var byteList = new List<byte>();
         byte[] length = BitConverter.GetBytes(5).Reverse().ToArray();
         byte[] bytePIndex = BitConverter.GetBytes(piece.Index).Reverse().ToArray();

         byteList.AddRange(length);
         byteList.Add((byte)TorrentMessageStatus.Have);
         byteList.AddRange(bytePIndex);
         GetSocket().Send(byteList.ToArray());
      }

      # endregion

      # region Overrides

      public override bool Equals(object obj)
      {
         if (Id != null && ((TorrentPeer)obj).Id != null)
         {
            return Id == ((TorrentPeer)obj).Id;
         }
         return
            IpAddress == ((TorrentPeer)obj).IpAddress &&
            Port == ((TorrentPeer)obj).Port;
      }

      # endregion
   }
}
