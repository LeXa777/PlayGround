using System;

namespace BitTorrent.Model
{
   public class TorrentException : Exception
   {
      public Torrent Torrent;

      public TorrentException(Torrent torrent, string message) : base(message)
      {
         Torrent = torrent;
      }

      public TorrentException(Torrent torrent, Exception innerException, string message)
         : base(message, innerException)
      {
         Torrent = torrent;
      }
   }
}
