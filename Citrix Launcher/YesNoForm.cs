using System;
using System.Drawing;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class YesNoForm : Form
    {
        private string prompt;
        private Image image;
        private IYesNoHandler handler;

        #region Form
        public YesNoForm(string prompt, Image image, IYesNoHandler handler)
        {
            this.prompt = prompt;
            this.handler = handler;
            this.image = image;

            InitializeComponent();
        }
        #endregion

        private void YesNoForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.dv_icon;
            Text = Properties.Strings.windowTitle;
            yesNoPictureBox.Image = image;
            yesNoBodyText.Text = prompt;
            yesButton.Text = Properties.Strings.buttonYes;
            noButton.Text = Properties.Strings.buttonNo;
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            handler.YesNoHandler(true, prompt, this);
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            handler.YesNoHandler(false, prompt, this);
        }
    }
}