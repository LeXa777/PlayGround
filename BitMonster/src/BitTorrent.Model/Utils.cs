using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BitTorrent.Model.Bencode;

namespace BitTorrent.Model
{
   public static class Utils
   {
      private static readonly Encoding _extendedASCIIEncoding = Encoding.GetEncoding(437);

      public static string ConvertBytesToFriendlyString(long bytes)
      {
         return string.Format("{0:#,##0}KB", bytes);
      }

      public static string Classname(this object obj)
      {
         return obj.GetType().Name;
      }

      public static string GetText(byte[] byteStr)
      {
         return _extendedASCIIEncoding.GetString(byteStr);
      }

      public static string GetText(byte[] byteStr, int index, int count)
      {
         return _extendedASCIIEncoding.GetString(byteStr, index, count);
      }

      public static byte[] GetBytes(string str)
      {
         return _extendedASCIIEncoding.GetBytes(str);
      }

      public static string GetUtf8String(string str)
      {
         return Encoding.UTF8.GetString(GetBytes(str));
      }

      public static Image GetBitfieldImage(bool[] bitfield)
      {
         var bitmap = new Bitmap(100, 200);
         var brush = new SolidBrush(Color.Teal);

         using (var g = Graphics.FromImage(bitmap))
         {
            g.Clear(Color.Tomato);
            g.FillEllipse(brush, 10, 10, 5, 5);
         }

         return bitmap;
      }

      public static string GetBitfieldString(bool[] bitfield)
      {
         var sb = new StringBuilder();

         if (bitfield != null)
         {
            foreach (var b in bitfield)
            {
               sb.Append(b ? "|" : ".");
            }
         }
         return sb.ToString();
      }

      public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
      {
         var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
         dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
         return dtDateTime;
      }

      public static IBencode Parse(string str)
      {
         var bytes = GetBytes(str);
         return Parse(new MemoryStream(bytes));
      }

      public static IBencode Parse(Stream stream)
      {
         using (var sr = new BinaryReader(stream, _extendedASCIIEncoding))
         {
            return Parse(sr);
         }
      }

      public static IBencode Parse(FileInfo file)
      {
         using (var fs = file.OpenRead())
         {
            return Parse(fs);
         }
      }

      public static IBencode Parse(BinaryReader inputStream)
      {
         var next = (char)inputStream.PeekChar();

         switch (next)
         {
            case 'i':
               return BencodeInteger.Parse(inputStream);
            case 'l':
               return BencodeList.Parse(inputStream);
            case 'd':
               return BencodeDictionary.Parse(inputStream);
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
               return BencodeString.Parse(inputStream);
         }

         return null;
      }

      public static string TrimString(string str)
      {
         if (!string.IsNullOrEmpty(str))
         {
            return str.Trim().Replace("\n", string.Empty).Replace("\r", string.Empty);
         }
         return string.Empty;
      }

      public static byte[] CalculateTorrentInfoHash(IBencode bCode)
      {
         byte[] infoBytes = EncodeBytes(bCode);

         // Hash the encoded dictionary
         return new SHA1CryptoServiceProvider().ComputeHash(infoBytes);
      }

      public static byte[] EncodeBytes(IBencode bCode)
      {
         var ms = new MemoryStream();
         var bw = new BinaryWriter(ms, _extendedASCIIEncoding);
         bCode.Encode(bw);
         ms.Position = 0;
         return new BinaryReader(ms, _extendedASCIIEncoding).ReadBytes((int) ms.Length);
      }

      public static uint ByteArrToUInt(byte[] bytes, int start, int length)
      {
         byte[] lenBytes = new byte[length];
         Array.Copy(bytes, start, lenBytes, 0, length);
         return ByteArrToUInt(lenBytes);
      }

      public static uint ByteArrToUInt(byte[] bytes)
      {
         if (bytes != null && (bytes.Length == 2 || bytes.Length == 4))
         {
            byte[] arr = new byte[bytes.Length];
            Array.Copy(bytes, 0, arr, 0, bytes.Length);

            if (BitConverter.IsLittleEndian)
            {
               arr = arr.Reverse().ToArray();
            }

            if (bytes.Length == 2)
            {
               return BitConverter.ToUInt16(arr, 0);
            }
            else if (bytes.Length == 4)
            {
               return BitConverter.ToUInt32(arr, 0);
            }
         }
         return 0;
      }

      public static byte[] ParseBitFieldIntoBytes(bool[] bitfield)
      {
         int pieces = bitfield.Length / 8;

         if (bitfield.Length % 8 != 0)
         {
            pieces++;
         }

         var ret = new byte[pieces];

         for (int i = 0; i < bitfield.Length; i += 8)
         {
            var arr = new BitArray(8);
            for (int j = 0; j < 8; j++)
            {
               int pos = i + j;
               if (pos < bitfield.Length)
               {
                  arr[7 - j] = bitfield[pos];
               }
            }
            arr.CopyTo(ret, i != 0 ? i / 8 : 0);
         }

         return ret;
      }

      public static bool[] ParseBitfield(byte[] bitfield, int length)
      {
         var ret = new bool[length];

         try
         {
            if (bitfield != null)
            {
               int next = 0;
               for (int i = 0; i < length; i += 8)
               {
                  byte b = bitfield[next];
                  next++;

                  for (int j = 0; j < 8; j++)
                  {
                     int num = i + j;
                     if (num < ret.Length)
                     {
                        bool res = IsBitSet(b, 7 - j);
                        ret[i + j] = res;
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {

         }

         return ret;
      }

      private static bool IsBitSet(byte b, int index)
      {
         int mask = 1 << index;
         return (b & mask) != 0;
      }
   }
}
