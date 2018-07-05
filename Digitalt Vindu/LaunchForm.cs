using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Digitalt_Vindu
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

            Thread processCheck = new Thread(LookForCTXProcess);
            processCheck.Start();
        }

        private void LookForCTXProcess()
        {
            int count = 0;
            Process[] p = Process.GetProcessesByName("CDViewer");

            // run while process does not exist
            while (p.Length == 0 && count < CoreForm.dvLaunchTimeout)
            {
                Thread.Sleep(1000);
                p = Process.GetProcessesByName("CDViewer");
                count++;
            }

            Application.Exit();
        }
    }
}