using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BitTorrent.Model;
using NUnit.Framework;

namespace BitTorrent.Tests.Client
{
   [TestFixture]
   public class TestTorrentParser
   {
      [Test]
      public void SimpleTest()
      {
         var data = "d4:alex5:vasyae";
         var bytesBefore = Utils.GetBytes(data);
         var res = Utils.Parse(data);
         var bytesAfter = Utils.EncodeBytes(res);
         var result = Utils.GetText(bytesAfter);
         Assert.AreEqual(data, result);
      }
   }
}
