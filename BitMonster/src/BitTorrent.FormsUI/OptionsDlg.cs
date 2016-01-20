using System;
using System.Windows.Forms;
using BitTorrent.Model;

namespace BitTorrent.FormsUI
{
   public partial class OptionsDlg : Form
   {
      public string SaveDir
      { get { return txtSaveDir.Text; } }
      public OptionsDlg()
      {
         InitializeComponent();
      }

      private void btnOpenDir_Click(object sender, EventArgs e)
      {
         var res = folderBrowserDialog.ShowDialog(this);

         if (res == DialogResult.OK)
         {
            txtSaveDir.Text = folderBrowserDialog.SelectedPath;
         }
      }

      private void btnSave_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.OK;
         this.Close();
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.Cancel;
         this.Close();
      }

      private void OptionsDlg_Load(object sender, EventArgs e)
      {
         if (!string.IsNullOrEmpty(TorrentApplication.SaveDir))
         {
            folderBrowserDialog.SelectedPath = TorrentApplication.SaveDir;
         }

         txtSaveDir.Text = TorrentApplication.SaveDir;
      }

      private void OptionsDlg_FormClosing(object sender, FormClosingEventArgs e)
      {
         this.DialogResult = DialogResult.Cancel;
      }
   }
}
