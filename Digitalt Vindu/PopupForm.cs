using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Digitalt_Vindu
{
    public partial class PopupForm : Form
    {
        string browser = CoreForm.dvBrowserOrURL;
        string url = CoreForm.dvBrowserArgs;

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

        private void PopupForm_Load(object sender, EventArgs e)
        {
            text.Text = Properties.Strings.popupText;
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