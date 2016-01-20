namespace CommonControls
{
   partial class WaitDlg
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
         this.btnClose = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // btnClose
         // 
         this.btnClose.Location = new System.Drawing.Point(91, 5);
         this.btnClose.Name = "btnClose";
         this.btnClose.Size = new System.Drawing.Size(105, 31);
         this.btnClose.TabIndex = 0;
         this.btnClose.Text = "Close";
         this.btnClose.UseVisualStyleBackColor = true;
         this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
         // 
         // WaitDlg
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(304, 40);
         this.Controls.Add(this.btnClose);
         this.Name = "WaitDlg";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "WaitDlg";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnClose;
   }
}