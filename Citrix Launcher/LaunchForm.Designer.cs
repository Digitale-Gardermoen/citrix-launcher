namespace citrix_launcher
{
    partial class LaunchForm
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
            this.launch = new System.Windows.Forms.PictureBox();
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.launch)).BeginInit();
            this.SuspendLayout();
            // 
            // launch
            // 
            this.launch.Location = new System.Drawing.Point(1, 0);
            this.launch.Name = "launch";
            this.launch.Size = new System.Drawing.Size(150, 150);
            this.launch.TabIndex = 0;
            this.launch.TabStop = false;
            // 
            // loadingBar
            // 
            this.loadingBar.Location = new System.Drawing.Point(1, 153);
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(150, 10);
            this.loadingBar.TabIndex = 1;
            this.loadingBar.UseWaitCursor = true;
            // 
            // LaunchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(150, 163);
            this.Controls.Add(this.loadingBar);
            this.Controls.Add(this.launch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LaunchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DeepSkyBlue;
            this.Load += new System.EventHandler(this.LaunchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.launch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox launch;
        private System.Windows.Forms.ProgressBar loadingBar;
    }
}