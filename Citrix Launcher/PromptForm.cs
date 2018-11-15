using System;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class PromptForm : Form
    {
        private int launchTimeout;
        private string launchCTXClientPath;
        private string launchCTXClientArgs;

        #region Form
        public PromptForm(int timeout, string path, string args)
        {
            launchTimeout = timeout;
            launchCTXClientPath = path;
            launchCTXClientArgs = args;

            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
        #endregion

        private void PromptForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.dv_icon;
            Text = Properties.Strings.promptWindowTitle;
            pictureBox.Image = Properties.Resources.dv_launch_50x50;
            bodyText.Text = Properties.Strings.promptText;
            yesButton.Text = Properties.Strings.buttonYes;
            noButton.Text = Properties.Strings.buttonNo;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            new LaunchForm(launchTimeout, launchCTXClientPath, launchCTXClientArgs);
            Application.Exit();
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            new LaunchForm(launchTimeout, launchCTXClientPath, launchCTXClientArgs);
            Application.Exit();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
