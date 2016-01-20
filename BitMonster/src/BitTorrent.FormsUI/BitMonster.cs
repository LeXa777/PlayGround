using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using BitTorrent.Model;
using System.Diagnostics;

namespace BitTorrent.FormsUI
{
   [Serializable]
   public partial class BitMonster : Form
   {
      [NonSerialized]
      private object _serializeLock = new object();

      private TorrentApplication _torrentApplication;
      private ListViewItem _lastSelectedTorrent;

      public BitMonster(string file)
      {
         _torrentApplication = new TorrentApplication();
         InitializeComponent();

         this.Text += string.Format(" [v{0}]", TorrentApplication.Version);

         if (!string.IsNullOrEmpty(file))
         {
            AddTorrent(file);
         }
      }

      # region UI Event Handlers

      private void addTorrentToolStripMenuItem_Click_1(object sender, EventArgs e)
      {
         AddTorrent();
      }

      private void addToolStripMenuItem_Click(object sender, EventArgs e)
      {
         AddTorrent();
      }

      private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
      {
         DownloadTorrent();
         Serialize();
      }

      private void lstTorrent_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
      {
         TorrentSelectionChanged();
      }

      private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
      {
         DeleteTorrent();
      }

      private void btnSerialize_Click(object sender, EventArgs e)
      {
         Serialize();
      }

      private void AlexTorrent_Load(object sender, EventArgs e)
      {
         Deserialize();
      }

      private void btnDeleteState_Click(object sender, EventArgs e)
      {
         DeleteState();
      }

      private void chkDisplayHandshaked_CheckedChanged(object sender, EventArgs e)
      {
         var torrents = GetSelectedTorrents();

         if (torrents.Count == 1)
         {
            UpdatePeersList(torrents[0].Peers);
         }
      }

      private void addPeerToolStripMenuItem_Click(object sender, EventArgs e)
      {
         AddPeer();
      }

      private void deletePeerToolStripMenuItem_Click(object sender, EventArgs e)
      {
         var peers = GetSelectedPeers();
         var torrents = GetSelectedTorrents();
         if (peers.Count == 1 && torrents.Count == 1)
         {
            torrents[0].DeletePeer(peers[0]);
         }
      }

      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         var torrents = GetSelectedTorrents();

         if (torrents.Count == 1)
         {
            var file = Path.Combine(TorrentApplication.SaveDir, torrents[0].Files[0].Path);
            if (File.Exists(file))
            {
               Process.Start(file);
            }
         }
      }

      private void AlexTorrent_FormClosing(object sender, FormClosingEventArgs e)
      {
         //Task.Factory.StartNew(() =>
         //{
            try
            {
               TorrentApplication.Shutdown();
               Application.Exit();
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }
            finally
            {
               Serialize();
            }
         //});
      }

      # endregion

      # region Helpers

      private void DownloadTorrent()
      {
         var torrents = GetSelectedTorrents();

         if (torrents.Count == 1)
         {
            Download(torrents[0]);
         }
         else
         {
            Error("You must select a torrent and a peer");
         }
      }

      private void DeleteState()
      {
         var path = Path.Combine(TorrentApplication.GlobalLocation, "data");
         if (File.Exists(path))
         {
            try
            {
               File.Delete(path);
               Info("Deleted ok...");
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
               Error(ex.Message);
            }
         }
      }

      private void Serialize()
      {
         lock (_serializeLock)
         {
            try
            {
               var path = Path.Combine(TorrentApplication.GlobalLocation, "data");
               using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
               {
                  var bf = new BinaryFormatter();
                  bf.Serialize(fs, _torrentApplication);
                  bf.Serialize(fs, TorrentApplication.Torrents);
               }
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
               Error(ex.Message);
            }
         }
      }

      private void Deserialize()
      {
         var path = Path.Combine(TorrentApplication.GlobalLocation, "data");
         if (File.Exists(path))
         {
            try
            {
               using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
               {
                  var bf = new BinaryFormatter();
                  _torrentApplication = (TorrentApplication)bf.Deserialize(fs);
                  TorrentApplication.Torrents = (Dictionary<string, Torrent>)bf.Deserialize(fs);

                  foreach (var torrent in TorrentApplication.Torrents.Values)
                  {
                     listTorrent.Add(torrent);
                  }
               }

               foreach (var torrent in TorrentApplication.Torrents.Values)
               {
                  torrent.Updated += Updated;
                  torrent.Downloaded += torrent_Downloaded;
               }

               TorrentApplication.WakeUp();

//               UpdateTorrentList();
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
               Error(ex.Message);
            }
         }
      }

      private void TorrentSelectionChanged()
      {
         UpdateSelectedTorrent();
      }

      private void UpdateSelectedTorrent()
      {
         var selectedItems = listTorrent.SelectedItems;

         if (selectedItems.Count == 1)
         {
            _lastSelectedTorrent = selectedItems[0];
         }

         if (_lastSelectedTorrent != null)
         {
            var t = _lastSelectedTorrent.Tag as Torrent;

            //listTorrent.Refresh(t);

            if (t != null)
            {
               UpdateFilesList(t.Files);
               UpdatePeersList(t.Peers);
            }
         }
      }

      private void UpdateTorrentList()
      {
         listTorrentFiles.Items.Clear();
         listTorrentPeers.Items.Clear();


//         foreach (var t in TorrentApplication.Torrents.Values)
//         {
//            listTorrent.Refresh(t);
//         }
      }

      private void UpdateFilesList(IList<TorrentFile> files)
      {
         listTorrentFiles.Items.Clear();

         foreach (var file in files)
         {
            var lvi = new ListViewItem(file.Length.ToString());
            lvi.SubItems.Add(file.Path);
            lvi.Tag = file;
            listTorrentFiles.Items.Add(lvi);
         }
      }

      private void UpdatePeersList(IList<TorrentPeer> peers)
      {
         listTorrentPeers.Items.Clear();

         foreach (var peer in peers.ToArray())
         {
            if ((chkDisplayHandshaked.Checked && peer.Status == TorrentPeerStatus.HandshakeOk) ||
                !chkDisplayHandshaked.Checked)
            {
               var lvi = new ListViewItem(peer.IpAddress);
               lvi.SubItems.Add(peer.Port.ToString());
               lvi.SubItems.Add(peer.Status.ToString());

               lvi.SubItems.Add(string.Format("{0:#,##0}KB/s", peer.DownSpeed > 0 ? peer.DownSpeed / 1000 : 0));
               lvi.SubItems.Add(string.Format("{0:#,##0}KB/s", peer.UpSpeed  > 0 ? peer.UpSpeed / 1000 : 0));

               var image = peer.GetBitfieldImage();
               lvi.SubItems.Add(image);

               lvi.Tag = peer;
               listTorrentPeers.Items.Add(lvi);

               var lbl = new Label { Font = listTorrentPeers.Font};
               lbl.Text = image;
               listTorrentPeers.Columns[5].Width = lbl.PreferredWidth /2;
            }
         }
      }

      private void AddTorrent()
      {
         if (openTorrentDlg.ShowDialog(this) == DialogResult.OK)
         {
            AddTorrent(openTorrentDlg.FileName);
         }
      }

      private void AddTorrent(string file)
      {
         var torrent = new Torrent(file);
         if (!TorrentApplication.Torrents.ContainsKey(Utils.GetText(torrent.InfoHashPlain)))
         {
            TorrentApplication.Torrents.Add(Utils.GetText(torrent.InfoHashPlain), torrent);
            torrent.Updated += Updated;
            torrent.Downloaded += torrent_Downloaded;
            listTorrent.Add(torrent);
         }
         else
         {
            Error("Torrent already added");
         }
         UpdateTorrentList();
         SelectTorrent(torrent);
      }

      private void SelectTorrent(Torrent torrent)
      {
         foreach (ListViewItem item in listTorrent.Items)
         {
            if (item.Tag == torrent)
            {
               item.Selected = true;
               listTorrent.Select();
               break;
            }
         }
      }

      private void DeleteTorrent()
      {
         var torrents = GetSelectedTorrents();

         if (torrents.Count > 0)
         {
            var torrentsArray = torrents.ToArray();
            foreach (var torrent in torrentsArray)
            {
               torrent.Stop();
               TorrentApplication.Torrents.Remove(Utils.GetText(torrent.InfoHashPlain));
               listTorrent.Remove(torrent);
            }
            UpdateTorrentList();
         }
         else
         {
            Error("No terrents selected");
         }
      }

      private IList<TorrentPeer> GetSelectedPeers()
      {
         var ret = new List<TorrentPeer>();

         foreach (ListViewItem item in listTorrentPeers.SelectedItems)
         {
            ret.Add(item.Tag as TorrentPeer);
         }

         return ret;
      }

      private IList<Torrent> GetSelectedTorrents()
      {
         var ret = new List<Torrent>();

         foreach (ListViewItem item in listTorrent.SelectedItems)
         {
            ret.Add(item.Tag as Torrent);
         }

         return ret;
      }

      private void Download(Torrent torrent)
      {
         torrent.Download();
      }

      void Updated(TorrentEvent torrentEvent)
      {
         if (this.InvokeRequired)
         {
            this.Invoke(new Action(UpdateSelectedTorrent));
         }
         else
         {
            UpdateSelectedTorrent();
         }
         Serialize();
      }

      void torrent_Downloaded(TorrentEvent tEvent)
      {
         MessageBox.Show(string.Format("Torrent {0} Downloaded!", tEvent.Torrent.TorrentFilename));
      }

      private void AddPeer()
      {
         var torrents = GetSelectedTorrents();

         if (torrents.Count == 1)
         {
            var dlg = new AddPeerDlg();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
               torrents[0].AddPeer(dlg.Address, dlg.Port);
            }
         }
         else
         {
            Error("No torrent selected");
         }
      }

      private void Error(string error)
      {
         MessageBox.Show(error, "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void Info(string info)
      {
         MessageBox.Show(info, "Info",
                  MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      # endregion

      private void listTorrent_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Delete)
         {
            DeleteTorrent();
         }
      }

      private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         var options = new OptionsDlg();
         var res = options.ShowDialog(this);

         if (res == DialogResult.OK)
         {
            TorrentApplication.SaveDir = options.SaveDir;
         }
      }

      private void listTorrent_SelectedIndexChanged(object sender, EventArgs e)
      {
         UpdateSelectedTorrent();
      }

      private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (Logger.File != null && File.Exists(Logger.File))
         {
            Process.Start(Logger.File);
         }
         else
         {
            Info("No log file exist.");
         }
      }
   }
}
