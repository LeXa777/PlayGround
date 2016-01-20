using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
using BitTorrent.Model;
using System.Diagnostics;

namespace BitTorrent.FormsUI
{
   public class TorrentList : ListView
   {
      private ListViewItem _lastSelectedItem;
      private ToolTip _toolTip;

      public TorrentList()
      {
         InitTorrentList();
         this.Enter += TorrentList_Enter;
         this.ItemSelectionChanged += TorrentList_ItemSelectionChanged;
         this.ItemMouseHover += TorrentList_ItemMouseHover;
      }

      void TorrentList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
      {
         _toolTip = new ToolTip(this, (Torrent)e.Item.Tag);
      }

      void TorrentList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
      {
         if (SelectedItems.Count == 1)
         {
            SelectedItems[0].BackColor = Color.DeepSkyBlue;           

            if (_lastSelectedItem != null)
            {
               _lastSelectedItem.BackColor = Color.White;
            }
            
            _lastSelectedItem = SelectedItems[0];
         }
         
         foreach (ToolStripItem menu in ContextMenuStrip.Items)
         {
            if (SelectedItems.Count == 0)
            {
               menu.Enabled = menu.Text == "Add";
            }
            else
            {
               menu.Enabled = true;
            }
         }
      }

      void TorrentList_Enter(object sender, EventArgs e)
      {
         if (SelectedItems.Count == 0)
         {
            foreach (ToolStripItem menu in ContextMenuStrip.Items)
            {
               if (menu.Text != "Add")
               {
                  menu.Enabled = false;
               }
            }
         }
      }

      private void InitTorrentList()
      {
         this.View = View.Details;
         this.FullRowSelect = true;
         this.Columns.AddRange(new []
         {
            new ColumnHeader{Text = "Name", Width = 150},
            new ColumnHeader{Text = "Size", Width = 80},
            new ColumnHeader{Text = "%", Width = 80},
            new ColumnHeader{Text = "Peers", Width = 60}, 
            new ColumnHeader{Text = "Added", Width = 120}, 
            new ColumnHeader{Text = "Down Speed", Width = 100}, 
            new ColumnHeader{Text = "Up Speed", Width = 100},
            new ColumnHeader{Text = "Downloaded", Width = 100}, 
            new ColumnHeader{Text = "Uploaded", Width = 100}, 
         });
      }

      public void Add(Torrent torrent)
      {
         var lvi = new ListViewItem(torrent.TorrentName);
         lvi.SubItems.Add(Utils.ConvertBytesToFriendlyString(TorrentFile.FilesTotalLength(torrent.Files)));
         lvi.SubItems.Add(torrent.Progress + "%");
         lvi.SubItems.Add(torrent.ActivePeers.ToString());
         lvi.SubItems.Add(torrent.AddedDate.ToLongDateString());

         lvi.SubItems.Add(Utils.ConvertBytesToFriendlyString(torrent.DownSpeed));
         lvi.SubItems.Add(Utils.ConvertBytesToFriendlyString(torrent.UpSpeed));

         lvi.SubItems.Add(Utils.ConvertBytesToFriendlyString(torrent.DownloadedBytes));
         lvi.SubItems.Add(Utils.ConvertBytesToFriendlyString(torrent.UploadedBytes));
         lvi.Tag = torrent;
         
         Items.Add(lvi);

         torrent.Updated += torrent_Updated;
      }

      private void Update(ListViewItem item)
      {
         var torrent = (Torrent)item.Tag;

         item.SubItems[0].Text = torrent.TorrentName;
         item.SubItems[1].Text = Utils.ConvertBytesToFriendlyString(TorrentFile.FilesTotalLength(torrent.Files));
         item.SubItems[2].Text = torrent.Progress + "%";
         item.SubItems[3].Text = torrent.ActivePeers.ToString();
         item.SubItems[4].Text = torrent.AddedDate.ToLongDateString();
         item.SubItems[5].Text = Utils.ConvertBytesToFriendlyString(torrent.DownSpeed);
         item.SubItems[6].Text = Utils.ConvertBytesToFriendlyString(torrent.UpSpeed);
         item.SubItems[7].Text = Utils.ConvertBytesToFriendlyString(torrent.DownloadedBytes);
         item.SubItems[8].Text = Utils.ConvertBytesToFriendlyString(torrent.UploadedBytes);
      }

      public void Remove(Torrent torrent)
      {
         for (int i = Items.Count -1; i >= 0; i--)
         {
            if (Items[i].Tag == torrent)
            {
               ((Torrent)Items[i].Tag).Updated -= torrent_Updated;
               Items[i].Remove();
               break;
            }
         }
      }

      private void Refresh(Torrent torrent)
      {
         foreach (ListViewItem item in this.Items)
         {
            if (item.Tag == torrent)
            {
               Update(item);
               break;
            }
         }
      }

      void torrent_Updated(TorrentEvent peerEvent)
      {
         if (this.InvokeRequired)
         {
            this.Invoke(new Action(() => Refresh(peerEvent.Torrent)));
         }
         else
         {
            Refresh(peerEvent.Torrent);
         }
      }
   }
}
