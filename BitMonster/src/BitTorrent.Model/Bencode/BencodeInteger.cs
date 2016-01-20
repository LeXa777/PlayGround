using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BitTorrent.Model.Bencode
{
   public class BencodeInteger : IBencode
   {
      public long Value { get; private set; }

      public override string ToString()
      {
         return Value.ToString();
      }

      public void Encode(BinaryWriter writer)
      {
         writer.Write('i');

         writer.Write(Value.ToString().ToCharArray());

         writer.Write('e');
      }

      public static BencodeInteger Parse(BinaryReader inputStream)
      {
         var num = string.Empty;
         inputStream.Read();
         char chr;
         while ((chr = inputStream.ReadChar()) != 'e')
         {
            num += chr;
         }

         return new BencodeInteger { Value = long.Parse(num)};
      }
   }
}
