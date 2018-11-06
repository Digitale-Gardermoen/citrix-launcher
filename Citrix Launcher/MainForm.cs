using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class MainForm : Form, IErrorDisplayer
    {
        public Configuration Config { get; private set; }

        private IConfigProvider configProvider;

        [DllImport("user32.dll")] internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        #region Form
        public MainForm(IConfigProvider provider)
        {
            configProvider = provider;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;

            base.OnLoad(e);
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            Config = configProvider.GetConfiguration();
            IntPtr wHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, Config.CtxWindowTitle);

            if (wHandle == IntPtr.Zero)
            {
                CheckIPandStartCTX();
            }
            else
            {
                ShowWindow(wHandle, 3);
                SetForegroundWindow(wHandle);
                Application.Exit();
            }
        }

        public void CheckIPandStartCTX()
        {
            var ipAddresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            Form formToShow = null;
            foreach (IPAddress adr in ipAddresses)
            {
                string ipAddress = adr.ToString();

                if (Regex.IsMatch(ipAddress, Config.IpRegexPattern))
                {
                    formToShow = new PromptForm(Config.LaunchTimeout, Config.CtxClientPath, Config.CtxClientArgs);
                    break;
                }
                else if (Regex.IsMatch(ipAddress, Config.IpRegexPattern))
                {
                    formToShow = new LaunchForm(Config.LaunchTimeout, Config.CtxClientPath, Config.CtxClientArgs);
                    break;
                }
            }

            if (formToShow == null)
            {
                formToShow = new PopupForm(Config.PopupBrowserOrURL, Config.PopupBrowserArgs);
            }

            formToShow.Show();
        }

        public void ExitWithError(string msg, int exitcode)
        {
            MessageBox.Show(this, msg + Environment.NewLine + Environment.NewLine + Properties.Strings.popupErrorBottomText,
                                  Properties.Strings.popupErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Environment.Exit(exitcode);
        }
    }
}