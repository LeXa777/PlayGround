using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BitTorrent.Model.Bencode
{
   public class BencodeString : IBencode
   {
      public string Value { get; set; }

      public override string ToString()
      {
         return Value;
      }

      public void Encode(BinaryWriter writer)
      {
         writer.Write(Utils.GetBytes(string.Format("{0}:{1}", Utils.GetBytes(Value).Length, Value)));
      }

      public static BencodeString Parse(BinaryReader inputStream)
      {
         var length = string.Empty;
         char chr;
         while ((chr = inputStream.ReadChar()) != ':')
         {
            length += chr;
         }

         var buffer = inputStream.ReadBytes(int.Parse(length));
         return new BencodeString {Value = Utils.GetText(buffer)};
      }
   }
}
