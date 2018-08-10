using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class LaunchForm : Form
    {
        private Process ctxProcess;

        public LaunchForm(Process p)
        {
            ctxProcess = p;
            InitializeComponent();
        }

        private void LaunchForm_Load(object sender, EventArgs e)
        {
            loadingBar.Style = ProgressBarStyle.Marquee;
            loadingBar.MarqueeAnimationSpeed = 30;
            launch.Image = Properties.Resources.dv_launch;

            Thread processCheck = new Thread(LookForCTXProcess);
            processCheck.Start();
        }

        private void LookForCTXProcess()
        {
            int count = 0;
            while (!isProcessReady() && count++ < CoreForm.launchTimeout)
            {
                Thread.Sleep(1000);
            }

            Application.Exit();
        }

        private bool isProcessReady()
        {
            Console.WriteLine(ctxProcess.ExitTime + " " + (ctxProcess.ExitCode.ToString()));
            List<Process> pList = new List<Process>();
            string[] processNames = {
                "CDViewer",
                "Citrix Receiver"
            };

            foreach (string pName in processNames)
            {
                pList.AddRange(Process.GetProcessesByName(pName));
            }

            if (pList.Count > 0 || (ctxProcess.ExitTime.Day == DateTime.Now.Day && !ctxProcess.ExitCode.ToString().Equals("0")))
            {
                return true;
            }

            return false;
        }
    }
}