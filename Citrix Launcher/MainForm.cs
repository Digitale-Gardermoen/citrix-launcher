﻿using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class MainForm : Form
    {
        #region Form
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Gjemmer form ved oppstart
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;

            base.OnLoad(e);
        }
        #endregion

        enum Zones { Zone1, Zone2, Zone3 };

        private void MainForm_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            IntPtr wHandle = CoreForm.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, CoreForm.ctxWindowTitle);
            if (wHandle == IntPtr.Zero)
            {
                CheckIPandStartCTX();
            }
            else
            {
                CoreForm.ShowWindow(wHandle, 3);
                CoreForm.SetForegroundWindow(wHandle);
                Application.Exit();
            }
        }

        public void CheckIPandStartCTX()
        {
            Zones currentZone;
            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();

            if (Regex.IsMatch(ipAddress, CoreForm.ipRegexPattern1)) { currentZone = Zones.Zone1; }
            else if (Regex.IsMatch(ipAddress, CoreForm.ipRegexPattern2)) { currentZone = Zones.Zone2; }
            else { currentZone = Zones.Zone3; }

            Form launchForm = new LaunchForm();
            Form popupForm = new PopupForm();

            switch (currentZone)
            {
                case Zones.Zone1:
                    launchForm.Show();
                    Process.Start(CoreForm.ctxClientPath, " " + CoreForm.ctxClientArgs1);
                    break;
                case Zones.Zone2:
                    launchForm.Show();
                    Process.Start(CoreForm.ctxClientPath, " " + CoreForm.ctxClientArgs2);
                    break;
                case Zones.Zone3:
                    popupForm.Show();
                    break;
            }
        }
    }
}