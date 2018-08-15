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
        private int ctxProcessExitCode = -1;

        private int timeout = 120;

        public LaunchForm( int timeout, string path, string args)
        {
            this.timeout = timeout;
            ctxProcess = Process.Start(path, args);
            ctxProcess.EnableRaisingEvents = true;
            ctxProcess.Exited += CitrixProcessExited;
            InitializeComponent();
        }

        private void CitrixProcessExited(object sender, EventArgs e)
        {
            if(sender == ctxProcess)
            {
                ctxProcessExitCode = ctxProcess.ExitCode;
                Console.WriteLine("ctxProc exitcode:" + ctxProcess.ExitCode);
            }
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
            while (!isProcessReady() && count++ < timeout)
            {
                Thread.Sleep(1000);
            }

            Application.Exit();
        }

        private bool isProcessReady()
        {
            List<Process> pList = new List<Process>();
            string[] processNames = {
                "CDViewer"
            };
            
            foreach (string pName in processNames)
            {
                Process[] procs = Process.GetProcessesByName(pName);
                foreach(var proc in procs)
                {
                    Console.WriteLine(proc.ProcessName + " -- " + proc.MainModule.FileName);
                }
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