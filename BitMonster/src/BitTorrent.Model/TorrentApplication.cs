using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
//using BitTorrent.BencodeUtils;

namespace BitTorrent.Model
{
   [Serializable]
   public class TorrentApplication
   {
      public static int Port { get; set; }
      public static Dictionary<string, Torrent> Torrents { get; set; }

      private static Version _version;
      private static string _globalLocation;
      public static string SaveDir 
      {
         get
         {
            if (_saveDir == null)
            {
               var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
               _saveDir = Path.Combine(myDocs, "BitMonster");
            }
            return _saveDir;
         }
         set { _saveDir = value; } 
      }

      public static string GlobalLocation
      {
         get
         {
            if (_globalLocation == null)
            {
               var path = Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%"), "BitMonster");
               var di = new DirectoryInfo(path);
               if (!di.Exists)
               {
                  di.Create();
               }
               _globalLocation = path;
            }
            return _globalLocation;
         }
      }

      public static bool Running { get; private set; }

      /// <summary>
      /// Get the highest version of any assembly that came as part of the application
      /// </summary>
      public static Version Version
      {
         get
         {
            if (_version == null)
            {
               if (ApplicationDeployment.IsNetworkDeployed)
               {
                  _version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
               }
               else
               {
                  Version tmp = null;
                  foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                  {
                     var ver = FileVersionInfo.GetVersionInfo(a.Location);

                     if (ver.CompanyName == "LeXa777")
                     {
                        var newVer = a.GetName().Version;
                        if (tmp == null)
                        {
                           tmp = newVer;
                        }
                        else if (tmp < newVer)
                        {
                           tmp = newVer;
                        }
                     }
                  }
                  _version = tmp;
               }
            }
            return _version;
         }
      }

      private static Socket _listener;
      //private static bool _running = true;
      private static string _saveDir;

      public TorrentApplication()
      {
         Torrents = new Dictionary<string, Torrent>();
      }

      static TorrentApplication()
      {
         Running = true;
         Port = 7882;
         new Thread(new ThreadStart(SetupListener)).Start();
//         Logger.Info("Stared", "TorrentApplication", "TorrentApplication");
      }

      private static void SetupListener()
      {
         try
         {
            IPHostEntry ipHost = Dns.GetHostEntry(string.Empty);
            IPAddress ipAddr = ipHost.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(ipAddr, Port));
            _listener.Listen(10);

            try
            {
               while (Running)
               {
                  try
                  {
                     Socket handler = _listener.Accept();

                     var peer = new TorrentPeer(handler);
                     peer.HandshakeReceived += peer_HandshakeReceived;
                  }
                  catch (SocketException sex)
                  {
                     if (sex.ErrorCode != 10004) //i.e. stopping socket
                     {
                        Logger.Error(sex);
                        throw sex;
                     }
                  }
               }
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
         }
      }

      public static void Shutdown()
      {
         Running = false;
         try
         {
            _listener.Close();

            foreach (var torrent in Torrents.Values)
            {
               foreach (var peer in torrent.Peers)
               {
                  peer.Disconnect();
               }
            }
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
         }
      }

      static void peer_HandshakeReceived(TorrentPeer peer)
      {
         Torrents[Utils.GetText(peer.InfoHash)].Peers.Add(peer);
      }

      public static void WakeUp()
      {
         foreach (var torrent in Torrents.Values)
         {
            Task.Factory.StartNew(torrent.WakeUp);
         }
      }
   }
}
