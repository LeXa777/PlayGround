using System.IO;
using System.Text;
using BitTorrent.Model;
using NUnit.Framework;

namespace BitTorrent.Tests.Client
{
   [TestFixture]
   public class TestGettingFilePath
   {
      private TorrentFile[] Files1
      {
         get
         {
            return new TorrentFile[]
            {
               new TorrentFile {GlobalOffset = 0,   Length = 110, Path = "1.txt"},
               new TorrentFile {GlobalOffset = 110, Length = 20,  Path = "2.txt"},
               new TorrentFile {GlobalOffset = 130, Length = 40,  Path = "3.txt"}, 
               new TorrentFile {GlobalOffset = 170, Length = 28,  Path = "4.txt"}, 
               new TorrentFile {GlobalOffset = 198, Length = 1,   Path = "5.txt"},
               new TorrentFile {GlobalOffset = 199, Length = 124, Path = "6.txt"}
            };
         }
      }

      # region Long Data
      private TorrentFile[] Files2
      {
         get
         {
            return new TorrentFile[]
            {
               new TorrentFile
               {
                  GlobalOffset = 0,
                  Length = 6356620,
                  Path =
                     "0.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 6356620,
                  Length = 2813068,
                  Path =
                     "1.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 9169688,
                  Length = 9602584,
                  Path =
                     "2.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 18772272,
                  Length = 6243736,
                  Path =
                     "3.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 25016008,
                  Length = 6765580,
                  Path =
                     "4.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 31781588,
                  Length = 7097356,
                  Path =
                     "5.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 38878944,
                  Length = 6970252,
                  Path =
                     "6.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 45849196,
                  Length = 9602584,
                  Path =
                     "7.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 55451780,
                  Length = 9602584,
                  Path =
                     "8.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 65054364,
                  Length = 9602584,
                  Path =
                     "9.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 74656948,
                  Length = 9838360,
                  Path =
                     "10.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 84495308,
                  Length = 9602584,
                  Path =
                     "11.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 94097892,
                  Length = 4339096,
                  Path =
                     "12.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 98436988,
                  Length = 6346252,
                  Path =
                     "13.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 104783240,
                  Length = 9602584,
                  Path =
                     "14.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 114385824,
                  Length = 9046552,
                  Path =
                     "15.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 123432376,
                  Length = 10021900,
                  Path =
                     "16.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 133454276,
                  Length = 3879820,
                  Path =
                     "17.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 137334096,
                  Length = 4212364,
                  Path =
                     "18.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 141546460,
                  Length = 9350284,
                  Path =
                     "19.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 150896744,
                  Length = 11807884,
                  Path =
                     "20.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 162704628,
                  Length = 4974220,
                  Path =
                     "21.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 167678848,
                  Length = 9602584,
                  Path =
                     "22.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 177281432,
                  Length = 2755096,
                  Path =
                     "23.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 180036528,
                  Length = 4011916,
                  Path =
                     "24.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 184048444,
                  Length = 9223564,
                  Path =
                     "25.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 193272008,
                  Length = 9602584,
                  Path =
                     "26.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 202874592,
                  Length = 9602584,
                  Path =
                     "27.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 212477176,
                  Length = 7744024,
                  Path =
                     "28.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 220221200,
                  Length = 5971852,
                  Path =
                     "29.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 226193052,
                  Length = 9550732,
                  Path =
                     "30.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 235743784,
                  Length = 4306060,
                  Path =
                     "31.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 240049844,
                  Length = 9602584,
                  Path =
                     "32.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 249652428,
                  Length = 9602584,
                  Path =
                     "33.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 259255012,
                  Length = 5942680,
                  Path =
                     "34.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 265197692,
                  Length = 3491596,
                  Path =
                     "35.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 268689288,
                  Length = 10820620,
                  Path =
                     "36.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 279509908,
                  Length = 9602584,
                  Path =
                     "37.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 289112492,
                  Length = 9602584,
                  Path =
                     "38.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 298715076,
                  Length = 9602584,
                  Path =
                     "39.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 308317660,
                  Length = 5419288,
                  Path =
                     "40.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 313736948,
                  Length = 2215948,
                  Path =
                     "41.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 315952896,
                  Length = 9290380,
                  Path =
                     "42.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 325243276,
                  Length = 12214540,
                  Path = "43.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 337457816,
                  Length = 9602584,
                  Path =
                     "44.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 347060400,
                  Length = 9699736,
                  Path =
                     "45.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 356760136,
                  Length = 9602584,
                  Path =
                     "46.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 366362720,
                  Length = 10127128,
                  Path =
                     "47.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 376489848,
                  Length = 9602584,
                  Path =
                     "48.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 386092432,
                  Length = 5418904,
                  Path =
                     "49.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 391511336,
                  Length = 5118220,
                  Path =
                     "50.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 396629556,
                  Length = 9602584,
                  Path =
                     "51.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 406232140,
                  Length = 6223000,
                  Path =
                     "52.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 412455140,
                  Length = 10594828,
                  Path =
                     "53.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 423049968,
                  Length = 9602584,
                  Path = "54.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 432652552,
                  Length = 3435160,
                  Path = "55.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 436087712,
                  Length = 9602584,
                  Path =
                     "56.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 445690296,
                  Length = 9602584,
                  Path =
                     "57.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 455292880,
                  Length = 4314904,
                  Path =
                     "58.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 459607784,
                  Length = 9602584,
                  Path =
                     "59.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 469210368,
                  Length = 4569496,
                  Path =
                     "60.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 473779864,
                  Length = 9693964,
                  Path =
                     "61.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 483473828,
                  Length = 4892044,
                  Path = "62.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 488365872,
                  Length = 11948428,
                  Path = "63.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 500314300,
                  Length = 3515788,
                  Path =
                     "64.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 503830088,
                  Length = 2613388,
                  Path =
                     "65.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 506443476,
                  Length = 9182092,
                  Path =
                     "66.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 515625568,
                  Length = 2622988,
                  Path =
                     "67.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 518248556,
                  Length = 9602584,
                  Path =
                     "68.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 527851140,
                  Length = 5302168,
                  Path =
                     "69.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 533153308,
                  Length = 7606156,
                  Path =
                     "70.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 540759464,
                  Length = 7900300,
                  Path =
                     "71.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 548659764,
                  Length = 9602584,
                  Path =
                     "72.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 558262348,
                  Length = 5292184,
                  Path =
                     "73.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 563554532,
                  Length = 9602584,
                  Path =
                     "74.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 573157116,
                  Length = 9602584,
                  Path =
                     "75.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 582759700,
                  Length = 3646360,
                  Path =
                     "76.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 586406060,
                  Length = 5204236,
                  Path = "77.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 591610296,
                  Length = 2091916,
                  Path =
                     "78.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 593702212,
                  Length = 8696332,
                  Path =
                     "79.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 602398544,
                  Length = 1476748,
                  Path =
                     "80.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 603875292,
                  Length = 7745548,
                  Path =
                     "81.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 611620840,
                  Length = 3844108,
                  Path =
                     "82.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 615464948,
                  Length = 7405708,
                  Path =
                     "83.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 622870656,
                  Length = 4276876,
                  Path =
                     "84.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 627147532,
                  Length = 9602584,
                  Path = "85.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 636750116,
                  Length = 3413272,
                  Path = "86.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 640163388,
                  Length = 5678860,
                  Path =
                     "87.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 645842248,
                  Length = 3258892,
                  Path =
                     "88.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 649101140,
                  Length = 9602584,
                  Path =
                     "89.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 658703724,
                  Length = 4629784,
                  Path =
                     "90.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 663333508,
                  Length = 9602584,
                  Path =
                     "91.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 672936092,
                  Length = 7252504,
                  Path =
                     "92.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 680188596,
                  Length = 9602584,
                  Path =
                     "93.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 689791180,
                  Length = 9323800,
                  Path =
                     "94.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 699114980,
                  Length = 4924684,
                  Path =
                     "95.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 704039664,
                  Length = 1580428,
                  Path =
                     "96.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 705620092,
                  Length = 9602584,
                  Path =
                     "97.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 715222676,
                  Length = 9214360,
                  Path =
                     "98.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 724437036,
                  Length = 3354124,
                  Path =
                     "99.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 727791160,
                  Length = 5831308,
                  Path =
                     "100.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 733622468,
                  Length = 5463436,
                  Path =
                     "101.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 739085904,
                  Length = 2279692,
                  Path =
                     "102.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 741365596,
                  Length = 1761292,
                  Path =
                     "103.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 743126888,
                  Length = 3154828,
                  Path =
                     "104.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 746281716,
                  Length = 5160844,
                  Path =
                     "105.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 751442560,
                  Length = 4314892,
                  Path =
                     "106.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 755757452,
                  Length = 2121868,
                  Path =
                     "107.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 757879320,
                  Length = 1007500,
                  Path =
                     "108.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 758886820,
                  Length = 8778124,
                  Path =
                     "109.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 767664944,
                  Length = 4955788,
                  Path =
                     "110.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 772620732,
                  Length = 1656076,
                  Path =
                     "111.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 774276808,
                  Length = 10205452,
                  Path =
                     "112.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 784482260,
                  Length = 2590348,
                  Path =
                     "113.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 787072608,
                  Length = 8013964,
                  Path =
                     "114.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 795086572,
                  Length = 4836748,
                  Path =
                     "115.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 799923320,
                  Length = 10795276,
                  Path =
                     "116.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 810718596,
                  Length = 9602584,
                  Path =
                     "117.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 820321180,
                  Length = 7775896,
                  Path =
                     "118.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 828097076,
                  Length = 1785484,
                  Path =
                     "119.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 829882560,
                  Length = 1619212,
                  Path =
                     "120.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 831501772,
                  Length = 9602584,
                  Path =
                     "121.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 841104356,
                  Length = 9602584,
                  Path =
                     "122.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 850706940,
                  Length = 1363480,
                  Path =
                     "123.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 852070420,
                  Length = 4879372,
                  Path =
                     "124.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 856949792,
                  Length = 5812492,
                  Path =
                     "125.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 862762284,
                  Length = 9602584,
                  Path =
                     "126.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 872364868,
                  Length = 9602584,
                  Path =
                     "127.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 881967452,
                  Length = 4587928,
                  Path =
                     "128.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 886555380,
                  Length = 4831372,
                  Path =
                     "129.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 891386752,
                  Length = 3543052,
                  Path =
                     "130.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 894929804,
                  Length = 4172428,
                  Path =
                     "131.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 899102232,
                  Length = 2866828,
                  Path =
                     "132.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 901969060,
                  Length = 3772684,
                  Path =
                     "133.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 905741744,
                  Length = 9558028,
                  Path =
                     "134.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 915299772,
                  Length = 3968140,
                  Path =
                     "135.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 919267912,
                  Length = 6266764,
                  Path =
                     "136.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 925534676,
                  Length = 7393420,
                  Path =
                     "137.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 932928096,
                  Length = 9602584,
                  Path =
                     "138.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 942530680,
                  Length = 8447512,
                  Path =
                     "139.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 950978192,
                  Length = 4316428,
                  Path =
                     "140.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 955294620,
                  Length = 9602584,
                  Path =
                     "141.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 964897204,
                  Length = 3545752,
                  Path =
                     "142.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 968442956,
                  Length = 9602584,
                  Path =
                     "143.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 978045540,
                  Length = 3528088,
                  Path =
                     "144.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 981573628,
                  Length = 12093580,
                  Path =
                     "145.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 993667208,
                  Length = 7224844,
                  Path =
                     "146.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1000892052,
                  Length = 5059084,
                  Path =
                     "147.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1005951136,
                  Length = 9602584,
                  Path =
                     "148.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1015553720,
                  Length = 7538584,
                  Path =
                     "149.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1023092304,
                  Length = 9602584,
                  Path =
                     "150.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1032694888,
                  Length = 4529176,
                  Path =
                     "151.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1037224064,
                  Length = 12259852,
                  Path =
                     "152.txt"
               },
               new TorrentFile
               {
                  GlobalOffset = 1049483916,
                  Length = 6978560,
                  Path = "153"
               },
            };
         }
      }

      # endregion

      # region Single File Test

      [Test]
      public void TestSingleFileLength()
      {
         var chunks = TorrentFile.GetFiles(Files1, 30, 20);
         Assert.AreEqual(1, chunks.Count);
      }

      [Test]
      public void TestSingleFileAmount()
      {
         var chunks = TorrentFile.GetFiles(Files1, 30, 20);
         Assert.AreEqual(20, chunks[0].Amount);
      }

      [Test]
      public void Test_1_SingleFileBoundaries()
      {
         const int amount = 110;
         var chunks = TorrentFile.GetFiles(Files1, 0, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("1.txt", chunks[0].Filename);
      }

      [Test]
      public void Test_2_SingleFileBoundaries()
      {
         const int amount = 20;
         var chunks = TorrentFile.GetFiles(Files1, 110, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("2.txt", chunks[0].Filename);
      }

      [Test]
      public void Test_3_SingleFileBoundaries()
      {
         const int amount = 40;
         var chunks = TorrentFile.GetFiles(Files1, 130, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("3.txt", chunks[0].Filename);
      }

      [Test]
      public void Test_4_SingleFileBoundaries()
      {
         const int amount = 28;
         var chunks = TorrentFile.GetFiles(Files1, 170, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("4.txt", chunks[0].Filename);
      }

      [Test]
      public void Test_5_SingleFileBoundaries()
      {
         const int amount = 1;
         var chunks = TorrentFile.GetFiles(Files1, 198, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("5.txt", chunks[0].Filename);
      }

      [Test]
      public void Test_6_SingleFileBoundaries()
      {
         const int amount = 2;
         var chunks = TorrentFile.GetFiles(Files1, 199, amount);
         Assert.AreEqual(1, chunks.Count);
         Assert.AreEqual(amount, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("6.txt", chunks[0].Filename);
      }

      # endregion

      # region Multi File Test

      [Test]
      public void TestMultipleFiles_1()
      {
         const int amount = 202;
         var chunks = TorrentFile.GetFiles(Files1, 0, amount);
         Assert.AreEqual(6, chunks.Count);
         Assert.AreEqual(110, chunks[0].Amount);
         Assert.AreEqual(0, chunks[0].FileOffset);
         Assert.AreEqual("1.txt", chunks[0].Filename);
      }

      [Test]
      public void TestMultipleFiles_2()
      {
         const int amount = 91;
         var chunks = TorrentFile.GetFiles(Files1, 109, amount);
         Assert.AreEqual(6, chunks.Count);

         //1.txt
         Assert.AreEqual(1, chunks[0].Amount);
         Assert.AreEqual(109, chunks[0].FileOffset);
         Assert.AreEqual("1.txt", chunks[0].Filename);

         //2.txt
         Assert.AreEqual(20, chunks[1].Amount);
         Assert.AreEqual(0, chunks[1].FileOffset);
         Assert.AreEqual("2.txt", chunks[1].Filename);

         //3.txt
         Assert.AreEqual(40, chunks[2].Amount);
         Assert.AreEqual(0, chunks[2].FileOffset);
         Assert.AreEqual("3.txt", chunks[2].Filename);

         //4.txt
         Assert.AreEqual(28, chunks[3].Amount);
         Assert.AreEqual(0, chunks[3].FileOffset);
         Assert.AreEqual("4.txt", chunks[3].Filename);

         //5.txt
         Assert.AreEqual(1, chunks[4].Amount);
         Assert.AreEqual(0, chunks[4].FileOffset);
         Assert.AreEqual("5.txt", chunks[4].Filename);

         //6.txt
         Assert.AreEqual(1, chunks[5].Amount);
         Assert.AreEqual(0, chunks[5].FileOffset);
         Assert.AreEqual("6.txt", chunks[5].Filename);
      }

      [Test]
      public void TestMultipleFiles_3()
      {
         const int amount = 3;
         long start = TorrentFile.FilesTotalLength(Files1) - amount;
         var chunks = TorrentFile.GetFiles(Files1, start, amount);
         Assert.AreEqual(1, chunks.Count);

         //6.txt
         Assert.AreEqual(amount, chunks[0].Amount);
         long pos = Files1[5].Length - amount;
         Assert.AreEqual(pos, chunks[0].FileOffset);
         Assert.AreEqual("6.txt", chunks[0].Filename);
      }

      [Test]
      [ExpectedException("System.ArgumentOutOfRangeException")]
      public void TestMultipleFiles_Exception_1()
      {
         const int amount = 3456346;
         var chunks = TorrentFile.GetFiles(Files1, -535, amount);
      }

      [Test]
      public void TestMultipleFiles_Exception_2()
      {
         long amount = TorrentFile.FilesTotalLength(Files1);
         var chunks = TorrentFile.GetFiles(Files1, 0, amount);
      }

      [Test]
      [ExpectedException("System.ArgumentOutOfRangeException")]
      public void TestMultipleFiles_Exception_3()
      {
         long amount = TorrentFile.FilesTotalLength(Files1);
         var chunks = TorrentFile.GetFiles(Files1, 0, amount + 1);
      }

      # endregion

      # region Delete Me

      [Test]
      public void TestMultipleFiles_Delete_Me()
      {
         const long start = 757071872;
         const long amount = 1048576;
         var chunks = TorrentFile.GetFiles(Files2, start, amount);

         long totFilesLength = 0;

         foreach (var chunk in chunks)
         {
            totFilesLength += chunk.Amount;
         }

         Assert.AreEqual(amount, totFilesLength);
      }

      # endregion
   }
}
