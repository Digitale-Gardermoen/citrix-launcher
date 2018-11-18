using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class PopupForm : Form
    {
        private string browser;
        private string url;

        #region Form
        public PopupForm(string browser, string url)
        {
            this.browser = browser;
            this.url = url;
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
        #endregion

        private void PopupForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
            Text = Properties.Strings.popupWindowTitle;
            popupPictureBox.Image = Properties.Resources.remote_50x50;
            popupBodyText.Text = Properties.Strings.popupText;
            yesButton.Text = Properties.Strings.buttonYes;
            noButton.Text = Properties.Strings.buttonNo;
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            Process.Start(browser, url);
            Application.Exit();
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            Process.Start(browser, url);
            Application.Exit();
        }
    }
}