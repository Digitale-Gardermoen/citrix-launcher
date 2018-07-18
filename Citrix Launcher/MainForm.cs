using System;
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

        enum Zones { Internal, Secure, External };

        private void MainForm_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            IntPtr wHandle = CoreForm.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, CoreForm.ctxWindowTitle);
            if (wHandle == IntPtr.Zero)
            {
                CheckIPandStartDV();
            }
            else
            {
                CoreForm.ShowWindow(wHandle, 3);
                CoreForm.SetForegroundWindow(wHandle);
                Application.Exit();
            }
        }

        public void CheckIPandStartDV()
        {
            Zones currentZone;
            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();

            if (Regex.IsMatch(ipAddress, CoreForm.ipRegexPattern1)) { currentZone = Zones.Internal; }
            else if (Regex.IsMatch(ipAddress, CoreForm.ipRegexPattern2)) { currentZone = Zones.Secure; }
            else { currentZone = Zones.External; }

            Form launchForm = new LaunchForm();
            Form popupForm = new PopupForm();

            switch (currentZone)
            {
                case Zones.Internal:
                    launchForm.Show();
                    Process.Start(CoreForm.ctxClientPath, " " + CoreForm.ctxClientArgs1);
                    break;
                case Zones.Secure:
                    launchForm.Show();
                    Process.Start(CoreForm.ctxClientPath, " " + CoreForm.ctxClientArgs2);
                    break;
                case Zones.External:
                    popupForm.Show();
                    break;
            }
        }
    }
}