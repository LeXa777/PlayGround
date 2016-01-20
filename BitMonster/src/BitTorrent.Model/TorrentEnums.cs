using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitTorrent.Model
{
   public enum TorrentMessageStatus : int
   {
      Choke = 0,
      Unchoke = 1,
      Interested = 2,
      Uninterested = 3,
      Have = 4,
      Bitfield = 5,
      Request = 6,
      Piece = 7,
      Cancel = 8,
      Extended = 20
   }

   [Serializable]
   public enum TorrentPeerStatus : int
   {
      Uknown = 0,
      Unaccessible = 1,
      HandshakeOk = 2,
      HandshakeFailed = 3,
      OK = 4
   }
}
