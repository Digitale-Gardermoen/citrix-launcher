using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Digitalt_Vindu
{
    public partial class CoreForm : Form
    {
        #region Form
        public CoreForm()
        {
            InitializeComponent();
        }

        // Gjemmer form ved oppstart
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;

            base.OnLoad(e);
        }
        #endregion

        // Importerer funksjoner fra Windows API for kontroll av vinduer
        [DllImport("user32.dll")] internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static string ctxClientPath;
        public static string ctxClientArgsInternal;
        public static string ctxClientArgsSecure;
        public static string ctxWindowTitle;
        public static string dvBrowserOrURL;
        public static string dvBrowserArgs;
        public static int dvLaunchTimeout;
        public static string dvRegexInternal;
        public static string dvRegexSecure;

        string cfgFilePath = @"C:\Windows\DigitaltVindu\digitalt_vindu.cfg";

        public struct CfgKeys
        {
            public const string CTX_CLIENT_ARGS_INTERNAL = @"CTX_CLIENT_ARGS_INTERNAL";
            public const string CTX_CLIENT_ARGS_SECURE = @"CTX_CLIENT_ARGS_SECURE";
            public const string CTX_CLIENT_PATH = @"CTX_CLIENT_PATH";
            public const string CTX_WINDOW_TITLE = @"CTX_WINDOW_TITLE";
            public const string DV_BROWSER_ARGS = @"DV_BROWSER_ARGS";
            public const string DV_BROWSER_OR_URL = @"DV_BROWSER_OR_URL";
            public const string DV_LAUNCH_TIMEOUT_IN_SECONDS = @"DV_LAUNCH_TIMEOUT_IN_SECONDS";
            public const string DV_REGEX_INTERNAL = @"DV_REGEX_INTERNAL";
            public const string DV_REGEX_SECURE = @"DV_REGEX_SECURE";
        }

        public void CoreForm_Load(object sender, EventArgs e)
        {
            LoadConfig(cfgFilePath);
            Form main = new MainForm();
                 main.Show();
        }

        public void LoadConfig(string cfgFile)
        {
            try
            {
                if (!File.Exists(cfgFilePath))
                {
                    MessageBox.Show(this, Properties.Strings.dvErrorCfgFileMissing + Environment.NewLine + Environment.NewLine +
                                          Properties.Strings.dvErrorDefaultText, Properties.Strings.dvErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Environment.Exit(2);
                }
                else
                {
                    using (StreamReader sr = new StreamReader(cfgFile))
                    {
                        var cfg = new Dictionary<string, string>();
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Length == 0 || line.Substring(0, 1).Equals("#"))
                            {
                                continue;
                            }
                            string[] lineParts = line.Split('=');
                            string key = lineParts[0].Trim();
                            string value = lineParts[1].Trim();

                            cfg.Add(key, value);
                        }
                        if (isConfigValid(cfg))
                        {
                            ctxClientArgsInternal = cfg[CfgKeys.CTX_CLIENT_ARGS_INTERNAL];
                            ctxClientArgsSecure = cfg[CfgKeys.CTX_CLIENT_ARGS_SECURE];
                            ctxClientPath = cfg[CfgKeys.CTX_CLIENT_PATH];
                            ctxWindowTitle = cfg[CfgKeys.CTX_WINDOW_TITLE];
                            dvLaunchTimeout = int.Parse(cfg[CfgKeys.DV_LAUNCH_TIMEOUT_IN_SECONDS]);
                            dvRegexInternal = cfg[CfgKeys.DV_REGEX_INTERNAL];
                            dvRegexSecure = cfg[CfgKeys.DV_REGEX_SECURE];
                            dvBrowserArgs = cfg[CfgKeys.DV_BROWSER_ARGS];
                            dvBrowserOrURL = cfg[CfgKeys.DV_BROWSER_OR_URL];
                        }
                        else
                        {
                            throw new Exception(Properties.Strings.dvErrorCfgFileInvalid);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, Properties.Strings.dvErrorCfgFileNotReadable + Environment.NewLine + Environment.NewLine +
                                      e.Message + Environment.NewLine + Environment.NewLine +
                                      Properties.Strings.dvErrorDefaultText, Properties.Strings.dvErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(1);
            }
        }

        private bool isConfigValid(Dictionary<string, string> cfg)
        {
            CfgKeys cfgKeys = new CfgKeys();
            bool valid = true;
            foreach(var field in typeof(CfgKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(cfgKeys).ToString()))
                {
                    valid = false;
                }
            }
            return valid;
        }
    }
}