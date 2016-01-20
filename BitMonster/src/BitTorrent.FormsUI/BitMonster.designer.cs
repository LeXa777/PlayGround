namespace BitTorrent.FormsUI
{
   partial class BitMonster
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
         this.components = new System.ComponentModel.Container();
         this.contextMenuTorrentList = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.tabTorrentInfo = new System.Windows.Forms.TabControl();
         this.tabInfo = new System.Windows.Forms.TabPage();
         this.listTorrentFiles = new System.Windows.Forms.ListView();
         this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.tabPeers = new System.Windows.Forms.TabPage();
         this.chkDisplayHandshaked = new System.Windows.Forms.CheckBox();
         this.listTorrentPeers = new System.Windows.Forms.ListView();
         this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colDownSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colUpSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colPieces = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.contextMenuPeerList = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.addPeerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.deletePeerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.button1 = new System.Windows.Forms.Button();
         this.openTorrentDlg = new System.Windows.Forms.OpenFileDialog();
         this.menuMainDlg = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.addTorrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
         this.btnSerialize = new System.Windows.Forms.Button();
         this.btnDeleteState = new System.Windows.Forms.Button();
         this.listTorrent = new BitTorrent.FormsUI.TorrentList();
         this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.contextMenuTorrentList.SuspendLayout();
         this.tabTorrentInfo.SuspendLayout();
         this.tabInfo.SuspendLayout();
         this.tabPeers.SuspendLayout();
         this.contextMenuPeerList.SuspendLayout();
         this.menuMainDlg.SuspendLayout();
         this.SuspendLayout();
         // 
         // contextMenuTorrentList
         // 
         this.contextMenuTorrentList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.downloadToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.openToolStripMenuItem});
         this.contextMenuTorrentList.Name = "contextMenuTorrentList";
         this.contextMenuTorrentList.Size = new System.Drawing.Size(148, 100);
         // 
         // addToolStripMenuItem
         // 
         this.addToolStripMenuItem.Name = "addToolStripMenuItem";
         this.addToolStripMenuItem.Size = new System.Drawing.Size(147, 24);
         this.addToolStripMenuItem.Text = "Add";
         this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
         // 
         // downloadToolStripMenuItem
         // 
         this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
         this.downloadToolStripMenuItem.Size = new System.Drawing.Size(147, 24);
         this.downloadToolStripMenuItem.Text = "Download";
         this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
         // 
         // deleteToolStripMenuItem
         // 
         this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
         this.deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 24);
         this.deleteToolStripMenuItem.Text = "Delete";
         this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
         // 
         // openToolStripMenuItem
         // 
         this.openToolStripMenuItem.Name = "openToolStripMenuItem";
         this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 24);
         this.openToolStripMenuItem.Text = "Open";
         this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
         // 
         // tabTorrentInfo
         // 
         this.tabTorrentInfo.Controls.Add(this.tabInfo);
         this.tabTorrentInfo.Controls.Add(this.tabPeers);
         this.tabTorrentInfo.Location = new System.Drawing.Point(4, 299);
         this.tabTorrentInfo.Name = "tabTorrentInfo";
         this.tabTorrentInfo.SelectedIndex = 0;
         this.tabTorrentInfo.Size = new System.Drawing.Size(903, 210);
         this.tabTorrentInfo.TabIndex = 2;
         // 
         // tabInfo
         // 
         this.tabInfo.Controls.Add(this.listTorrentFiles);
         this.tabInfo.Location = new System.Drawing.Point(4, 25);
         this.tabInfo.Name = "tabInfo";
         this.tabInfo.Padding = new System.Windows.Forms.Padding(3);
         this.tabInfo.Size = new System.Drawing.Size(895, 181);
         this.tabInfo.TabIndex = 0;
         this.tabInfo.Text = "Info";
         this.tabInfo.UseVisualStyleBackColor = true;
         // 
         // listTorrentFiles
         // 
         this.listTorrentFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
         this.listTorrentFiles.Dock = System.Windows.Forms.DockStyle.Fill;
         this.listTorrentFiles.FullRowSelect = true;
         this.listTorrentFiles.Location = new System.Drawing.Point(3, 3);
         this.listTorrentFiles.Name = "listTorrentFiles";
         this.listTorrentFiles.Size = new System.Drawing.Size(889, 175);
         this.listTorrentFiles.TabIndex = 0;
         this.listTorrentFiles.UseCompatibleStateImageBehavior = false;
         this.listTorrentFiles.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader5
         // 
         this.columnHeader5.Text = "Length";
         this.columnHeader5.Width = 115;
         // 
         // columnHeader6
         // 
         this.columnHeader6.Text = "Path";
         this.columnHeader6.Width = 727;
         // 
         // tabPeers
         // 
         this.tabPeers.Controls.Add(this.chkDisplayHandshaked);
         this.tabPeers.Controls.Add(this.listTorrentPeers);
         this.tabPeers.Location = new System.Drawing.Point(4, 25);
         this.tabPeers.Name = "tabPeers";
         this.tabPeers.Padding = new System.Windows.Forms.Padding(3);
         this.tabPeers.Size = new System.Drawing.Size(895, 181);
         this.tabPeers.TabIndex = 1;
         this.tabPeers.Text = "Peers";
         this.tabPeers.UseVisualStyleBackColor = true;
         // 
         // chkDisplayHandshaked
         // 
         this.chkDisplayHandshaked.AutoSize = true;
         this.chkDisplayHandshaked.Checked = true;
         this.chkDisplayHandshaked.CheckState = System.Windows.Forms.CheckState.Checked;
         this.chkDisplayHandshaked.Location = new System.Drawing.Point(714, 6);
         this.chkDisplayHandshaked.Name = "chkDisplayHandshaked";
         this.chkDisplayHandshaked.Size = new System.Drawing.Size(110, 21);
         this.chkDisplayHandshaked.TabIndex = 5;
         this.chkDisplayHandshaked.Text = "Handshaked";
         this.chkDisplayHandshaked.UseVisualStyleBackColor = true;
         this.chkDisplayHandshaked.CheckedChanged += new System.EventHandler(this.chkDisplayHandshaked_CheckedChanged);
         // 
         // listTorrentPeers
         // 
         this.listTorrentPeers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHost,
            this.colPort,
            this.colStatus,
            this.colDownSpeed,
            this.colUpSpeed,
            this.colPieces});
         this.listTorrentPeers.ContextMenuStrip = this.contextMenuPeerList;
         this.listTorrentPeers.Dock = System.Windows.Forms.DockStyle.Fill;
         this.listTorrentPeers.FullRowSelect = true;
         this.listTorrentPeers.Location = new System.Drawing.Point(3, 3);
         this.listTorrentPeers.Name = "listTorrentPeers";
         this.listTorrentPeers.Size = new System.Drawing.Size(889, 175);
         this.listTorrentPeers.TabIndex = 0;
         this.listTorrentPeers.UseCompatibleStateImageBehavior = false;
         this.listTorrentPeers.View = System.Windows.Forms.View.Details;
         // 
         // colHost
         // 
         this.colHost.Text = "Host";
         this.colHost.Width = 130;
         // 
         // colPort
         // 
         this.colPort.Text = "Port";
         this.colPort.Width = 124;
         // 
         // colStatus
         // 
         this.colStatus.Text = "Status";
         this.colStatus.Width = 111;
         // 
         // colDownSpeed
         // 
         this.colDownSpeed.Text = "Down Speed";
         this.colDownSpeed.Width = 104;
         // 
         // colUpSpeed
         // 
         this.colUpSpeed.Text = "Up Speed";
         this.colUpSpeed.Width = 90;
         // 
         // colPieces
         // 
         this.colPieces.Text = "Piecies";
         this.colPieces.Width = 168;
         // 
         // contextMenuPeerList
         // 
         this.contextMenuPeerList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPeerToolStripMenuItem,
            this.deletePeerToolStripMenuItem});
         this.contextMenuPeerList.Name = "contextMenuPeerList";
         this.contextMenuPeerList.Size = new System.Drawing.Size(156, 52);
         // 
         // addPeerToolStripMenuItem
         // 
         this.addPeerToolStripMenuItem.Name = "addPeerToolStripMenuItem";
         this.addPeerToolStripMenuItem.Size = new System.Drawing.Size(155, 24);
         this.addPeerToolStripMenuItem.Text = "Add Peer";
         this.addPeerToolStripMenuItem.Click += new System.EventHandler(this.addPeerToolStripMenuItem_Click);
         // 
         // deletePeerToolStripMenuItem
         // 
         this.deletePeerToolStripMenuItem.Name = "deletePeerToolStripMenuItem";
         this.deletePeerToolStripMenuItem.Size = new System.Drawing.Size(155, 24);
         this.deletePeerToolStripMenuItem.Text = "Delete Peer";
         this.deletePeerToolStripMenuItem.Click += new System.EventHandler(this.deletePeerToolStripMenuItem_Click);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(803, 515);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(100, 31);
         this.button1.TabIndex = 3;
         this.button1.Text = "Exit";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // openTorrentDlg
         // 
         this.openTorrentDlg.Filter = "Torrent files|*.torrent";
         // 
         // menuMainDlg
         // 
         this.menuMainDlg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.aboutToolStripMenuItem});
         this.menuMainDlg.Location = new System.Drawing.Point(0, 0);
         this.menuMainDlg.Name = "menuMainDlg";
         this.menuMainDlg.Size = new System.Drawing.Size(911, 28);
         this.menuMainDlg.TabIndex = 4;
         this.menuMainDlg.Text = "menuMainDlg";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTorrentToolStripMenuItem,
            this.exitToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
         this.fileToolStripMenuItem.Text = "File";
         // 
         // addTorrentToolStripMenuItem
         // 
         this.addTorrentToolStripMenuItem.Name = "addTorrentToolStripMenuItem";
         this.addTorrentToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
         this.addTorrentToolStripMenuItem.Text = "Add Torrent";
         this.addTorrentToolStripMenuItem.Click += new System.EventHandler(this.addTorrentToolStripMenuItem_Click_1);
         // 
         // exitToolStripMenuItem
         // 
         this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
         this.exitToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
         this.exitToolStripMenuItem.Text = "Exit";
         // 
         // toolsToolStripMenuItem
         // 
         this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.logFileToolStripMenuItem});
         this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
         this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
         this.toolsToolStripMenuItem.Text = "Tools";
         // 
         // optionsToolStripMenuItem
         // 
         this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
         this.optionsToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
         this.optionsToolStripMenuItem.Text = "Options";
         this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
         // 
         // aboutToolStripMenuItem
         // 
         this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
         this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
         this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
         this.aboutToolStripMenuItem.Text = "About";
         // 
         // aboutToolStripMenuItem1
         // 
         this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
         this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(119, 24);
         this.aboutToolStripMenuItem1.Text = "About";
         // 
         // btnSerialize
         // 
         this.btnSerialize.Location = new System.Drawing.Point(697, 515);
         this.btnSerialize.Name = "btnSerialize";
         this.btnSerialize.Size = new System.Drawing.Size(100, 31);
         this.btnSerialize.TabIndex = 3;
         this.btnSerialize.Text = "Serialize";
         this.btnSerialize.UseVisualStyleBackColor = true;
         this.btnSerialize.Click += new System.EventHandler(this.btnSerialize_Click);
         // 
         // btnDeleteState
         // 
         this.btnDeleteState.Location = new System.Drawing.Point(591, 515);
         this.btnDeleteState.Name = "btnDeleteState";
         this.btnDeleteState.Size = new System.Drawing.Size(100, 31);
         this.btnDeleteState.TabIndex = 3;
         this.btnDeleteState.Text = "Delete State";
         this.btnDeleteState.UseVisualStyleBackColor = true;
         this.btnDeleteState.Click += new System.EventHandler(this.btnDeleteState_Click);
         // 
         // listTorrent
         // 
         this.listTorrent.ContextMenuStrip = this.contextMenuTorrentList;
         this.listTorrent.FullRowSelect = true;
         this.listTorrent.Location = new System.Drawing.Point(8, 31);
         this.listTorrent.Name = "listTorrent";
         this.listTorrent.Size = new System.Drawing.Size(895, 262);
         this.listTorrent.TabIndex = 5;
         this.listTorrent.UseCompatibleStateImageBehavior = false;
         this.listTorrent.View = System.Windows.Forms.View.Details;
         this.listTorrent.SelectedIndexChanged += new System.EventHandler(this.listTorrent_SelectedIndexChanged);
         this.listTorrent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listTorrent_KeyDown);
         // 
         // logFileToolStripMenuItem
         // 
         this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
         this.logFileToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
         this.logFileToolStripMenuItem.Text = "Log File";
         this.logFileToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
         // 
         // BitMonster
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(911, 551);
         this.Controls.Add(this.listTorrent);
         this.Controls.Add(this.btnDeleteState);
         this.Controls.Add(this.btnSerialize);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.tabTorrentInfo);
         this.Controls.Add(this.menuMainDlg);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "BitMonster";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "BitMonster";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlexTorrent_FormClosing);
         this.Load += new System.EventHandler(this.AlexTorrent_Load);
         this.contextMenuTorrentList.ResumeLayout(false);
         this.tabTorrentInfo.ResumeLayout(false);
         this.tabInfo.ResumeLayout(false);
         this.tabPeers.ResumeLayout(false);
         this.tabPeers.PerformLayout();
         this.contextMenuPeerList.ResumeLayout(false);
         this.menuMainDlg.ResumeLayout(false);
         this.menuMainDlg.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TabControl tabTorrentInfo;
      private System.Windows.Forms.TabPage tabInfo;
      private System.Windows.Forms.TabPage tabPeers;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.OpenFileDialog openTorrentDlg;
      private System.Windows.Forms.MenuStrip menuMainDlg;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem addTorrentToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
      private System.Windows.Forms.ContextMenuStrip contextMenuTorrentList;
      private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
      private System.Windows.Forms.ListView listTorrentFiles;
      private System.Windows.Forms.ColumnHeader columnHeader5;
      private System.Windows.Forms.ColumnHeader columnHeader6;
      private System.Windows.Forms.Button btnSerialize;
      private System.Windows.Forms.Button btnDeleteState;
      private System.Windows.Forms.ListView listTorrentPeers;
      private System.Windows.Forms.ColumnHeader colHost;
      private System.Windows.Forms.ColumnHeader colPort;
      private System.Windows.Forms.ColumnHeader colStatus;
      private System.Windows.Forms.ContextMenuStrip contextMenuPeerList;
      private System.Windows.Forms.ColumnHeader colPieces;
      private System.Windows.Forms.CheckBox chkDisplayHandshaked;
      private System.Windows.Forms.ToolStripMenuItem addPeerToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem deletePeerToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
      private System.Windows.Forms.ColumnHeader colDownSpeed;
      private System.Windows.Forms.ColumnHeader colUpSpeed;
      private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
      private TorrentList listTorrent;
      private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
   }
}

