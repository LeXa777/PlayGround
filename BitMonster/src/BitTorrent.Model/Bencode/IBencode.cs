using System.IO;

namespace BitTorrent.Model.Bencode
{
   public interface IBencode
   {
      void Encode(BinaryWriter writer);
   }
}
