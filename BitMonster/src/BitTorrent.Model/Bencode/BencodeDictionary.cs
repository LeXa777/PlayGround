using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BitTorrent.Model.Bencode
{
   public class BencodeDictionary : Dictionary<string, IBencode>, IBencode
   {
      public static IBencode Parse(BinaryReader inputStream)
      {
         var ret = new BencodeDictionary();

         inputStream.Read();

         while (inputStream.PeekChar() != 'e')
         {
            var key = BencodeString.Parse(inputStream);
            var value = Utils.Parse(inputStream);

            ret.Add(key.Value, value);
         }

         inputStream.Read();

         return ret;
      }

      public void Encode(BinaryWriter writer)
      {
         writer.Write('d');

         foreach (var d in this)
         {
            var key = new BencodeString {Value = d.Key};
            key.Encode(writer);
            d.Value.Encode(writer);
         }

         writer.Write('e');
      }
   }
}
