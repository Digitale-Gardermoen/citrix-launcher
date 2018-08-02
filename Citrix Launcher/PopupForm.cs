using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class PopupForm : Form
    {
        #region Form

        public PopupForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
        #endregion

        string browser = CoreForm.popupBrowserOrURL;
        string url = CoreForm.popupBrowserArgs;

        private void PopupForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.dv_icon;
            Text = Properties.Strings.popupWindowTitle;
            popupPictureBox.Image = Properties.Resources.dv_remote_50x50;
            popupBodyText.Text = Properties.Strings.popupText;
            yesButton.Text = Properties.Strings.popupButtonYes;
            noButton.Text = Properties.Strings.popupButtonNo;
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