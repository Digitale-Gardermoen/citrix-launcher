namespace citrix_launcher
{
    partial class YesNoForm
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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.noButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.yesNoPictureBox = new System.Windows.Forms.PictureBox();
            this.yesNoBodyText = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yesNoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelButtons.Controls.Add(this.noButton);
            this.panelButtons.Controls.Add(this.yesButton);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 72);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(311, 33);
            this.panelButtons.TabIndex = 0;
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(231, 5);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(75, 23);
            this.noButton.TabIndex = 1;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // yesButton
            // 
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.Location = new System.Drawing.Point(153, 5);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // yesNoPictureBox
            // 
            this.yesNoPictureBox.Location = new System.Drawing.Point(12, 12);
            this.yesNoPictureBox.Name = "yesNoPictureBox";
            this.yesNoPictureBox.Size = new System.Drawing.Size(50, 50);
            this.yesNoPictureBox.TabIndex = 1;
            this.yesNoPictureBox.TabStop = false;
            // 
            // yesNoBodyText
            // 
            this.yesNoBodyText.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yesNoBodyText.Location = new System.Drawing.Point(69, 13);
            this.yesNoBodyText.Name = "yesNoBodyText";
            this.yesNoBodyText.Size = new System.Drawing.Size(242, 49);
            this.yesNoBodyText.TabIndex = 2;
            this.yesNoBodyText.Text = "Change text in Strings.resx";
            this.yesNoBodyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // YesNoForm
            // 
            this.ClientSize = new System.Drawing.Size(311, 105);
            this.Controls.Add(this.yesNoBodyText);
            this.Controls.Add(this.yesNoPictureBox);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesNoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change title in Strings.resx";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.YesNoForm_Load);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yesNoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.PictureBox yesNoPictureBox;
        private System.Windows.Forms.Label yesNoBodyText;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button yesButton;
    }
}