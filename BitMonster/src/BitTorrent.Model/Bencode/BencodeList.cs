using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BitTorrent.Model.Bencode
{
   public class BencodeList : List<IBencode>, IBencode
   {
      public static BencodeList Parse(BinaryReader inputStream)
      {
         var ret = new BencodeList();
         inputStream.Read();

         while (inputStream.PeekChar() != 'e')
         {
            ret.Add(Utils.Parse(inputStream));
         }

         inputStream.Read();
         return ret;
      }

      public override string ToString()
      {
         var sb = new StringBuilder();

         foreach (var l in this)
         {
            sb.Append(l);
         }

         return sb.ToString();
      }

      public void Encode(BinaryWriter writer)
      {
         writer.Write('l');

         foreach (var l in this)
         {
            l.Encode(writer);
         }

         writer.Write('e');
      }
   }
}
