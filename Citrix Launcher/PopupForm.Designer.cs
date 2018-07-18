namespace citrix_launcher
{
    partial class PopupForm
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
            this.popupPictureBox = new System.Windows.Forms.PictureBox();
            this.popupBodyText = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).BeginInit();
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
            // image
            // 
            this.popupPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.popupPictureBox.Location = new System.Drawing.Point(12, 12);
            this.popupPictureBox.Name = "image";
            this.popupPictureBox.Size = new System.Drawing.Size(50, 50);
            this.popupPictureBox.TabIndex = 1;
            this.popupPictureBox.TabStop = false;
            this.popupPictureBox.Click += new System.EventHandler(this.Picture_Click);
            // 
            // text
            // 
            this.popupBodyText.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupBodyText.Location = new System.Drawing.Point(69, 13);
            this.popupBodyText.Name = "text";
            this.popupBodyText.Size = new System.Drawing.Size(242, 49);
            this.popupBodyText.TabIndex = 2;
            this.popupBodyText.Text = "Change text in Strings.resx";
            this.popupBodyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PopupForm
            // 
            this.ClientSize = new System.Drawing.Size(311, 105);
            this.Controls.Add(this.popupBodyText);
            this.Controls.Add(this.popupPictureBox);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change title in Strings.resx";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PopupForm_Load);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.PictureBox popupPictureBox;
        private System.Windows.Forms.Label popupBodyText;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button yesButton;
    }
}