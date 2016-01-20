namespace BitTorrent.FormsUI
{
   partial class OptionsDlg
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
         this.label1 = new System.Windows.Forms.Label();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnSave = new System.Windows.Forms.Button();
         this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
         this.txtSaveDir = new System.Windows.Forms.TextBox();
         this.btnOpenDir = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(73, 17);
         this.label1.TabIndex = 0;
         this.label1.Text = "Save Path";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.btnOpenDir);
         this.groupBox1.Controls.Add(this.txtSaveDir);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Location = new System.Drawing.Point(12, 12);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(348, 259);
         this.groupBox1.TabIndex = 1;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Local Settings";
         // 
         // groupBox2
         // 
         this.groupBox2.Location = new System.Drawing.Point(366, 19);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(272, 252);
         this.groupBox2.TabIndex = 2;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "groupBox2";
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(532, 277);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(106, 30);
         this.btnCancel.TabIndex = 3;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnSave
         // 
         this.btnSave.Location = new System.Drawing.Point(420, 277);
         this.btnSave.Name = "btnSave";
         this.btnSave.Size = new System.Drawing.Size(106, 30);
         this.btnSave.TabIndex = 3;
         this.btnSave.Text = "Save";
         this.btnSave.UseVisualStyleBackColor = true;
         this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
         // 
         // folderBrowserDialog
         // 
         this.folderBrowserDialog.Description = "Choose where to save files";
         this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
         // 
         // txtSaveDir
         // 
         this.txtSaveDir.Location = new System.Drawing.Point(85, 15);
         this.txtSaveDir.Name = "txtSaveDir";
         this.txtSaveDir.Size = new System.Drawing.Size(223, 22);
         this.txtSaveDir.TabIndex = 1;
         // 
         // btnOpenDir
         // 
         this.btnOpenDir.Location = new System.Drawing.Point(314, 15);
         this.btnOpenDir.Name = "btnOpenDir";
         this.btnOpenDir.Size = new System.Drawing.Size(28, 23);
         this.btnOpenDir.TabIndex = 2;
         this.btnOpenDir.Text = "O";
         this.btnOpenDir.UseVisualStyleBackColor = true;
         this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
         // 
         // OptionsDlg
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(649, 314);
         this.Controls.Add(this.btnSave);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "OptionsDlg";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Options";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsDlg_FormClosing);
         this.Load += new System.EventHandler(this.OptionsDlg_Load);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnSave;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
      private System.Windows.Forms.Button btnOpenDir;
      private System.Windows.Forms.TextBox txtSaveDir;
   }
}