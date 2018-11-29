using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class LaunchForm : Form
    {
        private Process ctxProcess;
        private ILaunchTimeoutHandler lth;
        private Timer launchTimer;
        private int ctxProcessExitCode = -1;
        private int timeout = 30;

        public LaunchForm(int timeout, string path, string args, ILaunchTimeoutHandler lth)
        {
            this.timeout = timeout;
            this.lth = lth;
            ctxProcess = Process.Start(path, args);
            ctxProcess.EnableRaisingEvents = true;
            ctxProcess.Exited += CitrixProcessExited;
            launchTimer = new Timer();
            InitializeComponent();
        }

        private void CitrixProcessExited(object sender, EventArgs e)
        {
            if(sender == ctxProcess)
            {
                ctxProcessExitCode = ctxProcess.ExitCode;
            }
        }

        private void LaunchForm_Load(object sender, EventArgs e)
        {
            launch.Image = Properties.Resources.dv_launch;
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Maximum = timeout;

            InitiateLaunchTimer();
        }

        private void InitiateLaunchTimer()
        {
            launchTimer.Interval = 1000;
            launchTimer.Enabled = true;
            launchTimer.Start();

            launchTimer.Tick += new EventHandler(launchTimer_Tick);
        }

        private void launchTimer_Tick(object sender, EventArgs e)
        {
            if (progressBar.Value <= timeout && !IsProcessReady())
            {
                progressBar.Value++;
            }
            else
            {
                Application.Exit();
            }

            if (progressBar.Value >= timeout)
            {
                launchTimer.Stop();
                lth.CitrixLaunchTimedOut();
                this.Close();
            }
        }

        private bool IsProcessReady()
        {
            List<Process> pList = new List<Process>();
            string[] processNames = {
                "CDViewer",
                "SelfServicePlugin"
            };
            
            foreach (string pName in processNames)
            {
                Process[] procs = Process.GetProcessesByName(pName);
                pList.AddRange(procs);
            }

            if (pList.Count > 0 || (ctxProcess.HasExited && ctxProcessExitCode != 0))
            {
                return true;
            }

            return false;
        }
    }
}