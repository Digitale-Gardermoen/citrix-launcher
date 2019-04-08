using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class MainForm : Form, IErrorDisplayer, IYesNoHandler, ILaunchTimeoutHandler
    {
        public Configuration Config { get; private set; }

        private ILogHandler logger;
        private IConfigProvider configProvider;

        [DllImport("user32.dll")] internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")] internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #region Form
        public MainForm(IConfigProvider provider, ILogHandler logger)
        {
            this.logger = logger;
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

            if(!Config.useFallbackConfig)
            {
                IntPtr wHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, Config.CitrixWindowTitle);

                if (wHandle != IntPtr.Zero)
                {
                    ShowWindow(wHandle, 3);
                    SetForegroundWindow(wHandle);
                    Application.Exit();
                }
                else
                {
                    ClearCitrixCache();
                }
            }

            StartCitrix();
        }

        private void ClearCitrixCache()
        {
            if (Config.CitrixClearCache)
            {
                if (Directory.Exists(Config.CitrixCachePath))
                {
                    DirectoryInfo di = new DirectoryInfo(Config.CitrixCachePath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
        }

        public void StartCitrix()
        {
            Form formToShow = null;
            if (Config.didMatchIp)
            {
                if (Config.CitrixAutostart)
                {
                    formToShow = new LaunchForm(Config.LaunchTimeout, Config.CitrixClientPath, Config.CitrixClientArgs, this);
                }
                else
                {
                    formToShow = new YesNoForm(Properties.Strings.launchCitrixPrompt, Properties.Resources.dv_launch_50x50, this);
                }
            }
            else
            { 
                formToShow = new YesNoForm(Properties.Strings.launchHomeOfficePrompt, Properties.Resources.dv_remote_50x50, this);
            }

            formToShow.Show();
        }

        public void YesNoHandler(bool answerYes, string prompt, Form form)
        {
            form.Close();

            if (prompt == Properties.Strings.launchHomeOfficePrompt)
            {
                LaunchHomeOffice(answerYes);
            }
            else if (prompt == Properties.Strings.launchCitrixPrompt)
            {
                LaunchCitrix(answerYes);
            }
            else if (prompt == Properties.Strings.retryCitrixLaunchPrompt)
            {
                switch (answerYes)
                {
                    case true:
                        KillCitrixProcesses();
                        LaunchCitrix(answerYes);
                    break;

                    case false:
                        Application.Exit();
                    break;
                }
            }
        }

        private void LaunchHomeOffice(bool launch)
        {
            if (launch)
            {
                Process.Start(Config.BrowserPath, Config.BrowserURL);
            }

            Application.Exit();
        }

        private void LaunchCitrix(bool launch)
        {
            if (launch)
            {
                LaunchForm form = new LaunchForm(Config.LaunchTimeout, Config.CitrixClientPath, Config.CitrixClientArgs, this);
                form.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        public void CitrixLaunchTimedOut()
        {
            Form formToShow = new YesNoForm(Properties.Strings.retryCitrixLaunchPrompt, Properties.Resources.dv_launch_50x50, this);
            formToShow.Show();
        }

        private void KillCitrixProcesses()
        {
            string[] processNames = {
                "concentr",
                "Receiver",
                "SelfService",
                "SelfServicePlugin",
                "wfcrun32",
                "wfica32",
                "AuthManSvr",
                "CDViewer",
                "redirector"
            };

            try
            {
                foreach (string pName in processNames)
                {
                    foreach (var process in Process.GetProcessesByName(pName))
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write("ERROR! Failed to kill Citrix processes: " + e.TargetSite + " (" + e.Message + ")");
            }
        }

        public void ExitWithError(string msg, int exitcode)
        {
            MessageBox.Show(this, msg + Environment.NewLine + Environment.NewLine + Properties.Strings.errorBottomText,
                                  Properties.Strings.windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Environment.Exit(exitcode);
        }
    }
}