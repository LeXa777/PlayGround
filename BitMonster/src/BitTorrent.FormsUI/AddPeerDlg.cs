using System;
using System.Windows.Forms;

namespace BitTorrent.FormsUI
{
   public partial class AddPeerDlg : Form
   {
      public string Address { get; private set; }
      public int Port { get; private set; }

      public AddPeerDlg()
      {
         InitializeComponent();
      }

      private void btnAdd_Click(object sender, EventArgs e)
      {
         Address = txtIp.Text;
         Port = int.Parse(txtPort.Text);
         this.DialogResult = DialogResult.OK;
         this.Close();
      }
   }
}
