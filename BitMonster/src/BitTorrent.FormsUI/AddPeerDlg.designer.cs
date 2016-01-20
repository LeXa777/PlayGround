namespace BitTorrent.FormsUI
{
   partial class AddPeerDlg
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.txtIp = new System.Windows.Forms.TextBox();
         this.txtPort = new System.Windows.Forms.TextBox();
         this.btnAdd = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // txtIp
         // 
         this.txtIp.Location = new System.Drawing.Point(12, 13);
         this.txtIp.Name = "txtIp";
         this.txtIp.Size = new System.Drawing.Size(141, 22);
         this.txtIp.TabIndex = 0;
         this.txtIp.Text = "192.168.52.134";
         // 
         // txtPort
         // 
         this.txtPort.Location = new System.Drawing.Point(159, 13);
         this.txtPort.Name = "txtPort";
         this.txtPort.Size = new System.Drawing.Size(67, 22);
         this.txtPort.TabIndex = 1;
         this.txtPort.Text = "6881";
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(133, 45);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(93, 35);
         this.btnAdd.TabIndex = 2;
         this.btnAdd.Text = "Add";
         this.btnAdd.UseVisualStyleBackColor = true;
         this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
         // 
         // AddPeerDlg
         // 
         this.AcceptButton = this.btnAdd;
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(237, 86);
         this.Controls.Add(this.btnAdd);
         this.Controls.Add(this.txtPort);
         this.Controls.Add(this.txtIp);
         this.Name = "AddPeerDlg";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Add Peer";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtIp;
      private System.Windows.Forms.TextBox txtPort;
      private System.Windows.Forms.Button btnAdd;
   }
}