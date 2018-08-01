using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class LaunchForm : Form
    {
        public LaunchForm()
        {
            InitializeComponent();
        }

        private void LaunchForm_Load(object sender, EventArgs e)
        {
            loadingBar.Style = ProgressBarStyle.Marquee;
            loadingBar.MarqueeAnimationSpeed = 30;
            image.Image = null;

            Thread processCheck = new Thread(LookForCTXProcess);
            processCheck.Start();
        }

        private void LookForCTXProcess()
        {
            int count = 0;
            Process[] p = Process.GetProcessesByName("CDViewer");

            // run while process does not exist
            while (p.Length == 0 && count < CoreForm.launchTimeout)
            {
                Thread.Sleep(1000);
                p = Process.GetProcessesByName("CDViewer");
                count++;
            }

            Application.Exit();
        }
    }
}