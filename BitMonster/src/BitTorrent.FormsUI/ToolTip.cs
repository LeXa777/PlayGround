
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BitTorrent.Model;

namespace BitTorrent.FormsUI
{
   public class ToolTip : System.Windows.Forms.ToolTip
   {
      private Torrent _torrent;
      private Image _image;
      public ToolTip(Control control, Torrent torrent)
      {
         _torrent = torrent;
         this.OwnerDraw = true;
         this.Draw +=ToolTip_Draw;
         this.Popup += ToolTip_Popup;
         this.Disposed += ToolTip_Disposed;
         AutoPopDelay = 15000;
         InitialDelay = 1000;
         ReshowDelay = 500;
         ShowAlways = true;

         base.ToolTipTitle = torrent.TorrentName;
         base.SetToolTip(control, torrent.PublisherUrl);
         
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (_image != null)
         {
            _image.Dispose();
         }
      }

      void ToolTip_Disposed(object sender, System.EventArgs e)
      {
         
      }

      void ToolTip_Popup(object sender, PopupEventArgs e)
      {
         
      }

      void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
      {
         e.Graphics.DrawString(_torrent.Announce[0].AbsoluteUri, SystemFonts.DefaultFont, Brushes.Black, new Point());
 	      e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
         e.Graphics.DrawImage(_torrent.BitfieldImage, 0, 20);
      }
   }
}
