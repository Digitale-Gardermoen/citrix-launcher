using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class CoreForm : Form
    {
        string cfgPathParam;

        #region Form
        public CoreForm(string[] args)
        {
            if (args.Length == 2 && args[0].ToLower().Equals("-cfgpath"))
            {
                if (File.Exists(args[1]))
                {
                    cfgPathParam = args[1];
                }
            }
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

        [DllImport("user32.dll")] internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static string ctxClientPath;
        public static string ctxClientArgs1;
        public static string ctxClientArgs2;
        public static string ctxWindowTitle;
        public static string ipRegexPattern1;
        public static string ipRegexPattern2;
        public static int launchTimeout;
        public static string popupBrowserOrURL;
        public static string popupBrowserArgs;

        public string GetConfigPath()
        {
            if (cfgPathParam != null)
            {
                return cfgPathParam;
            }
            else
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Digitale Gardermoen IS\citrix-launcher\citrix-launcher.cfg";
            }
        }

        public struct CfgKeys
        {
            public const string CTX_CLIENT_ARGS1 = @"CTX_CLIENT_ARGS1";
            public const string CTX_CLIENT_ARGS2 = @"CTX_CLIENT_ARGS2";
            public const string CTX_CLIENT_PATH = @"CTX_CLIENT_PATH";
            public const string CTX_WINDOW_TITLE = @"CTX_WINDOW_TITLE";
            public const string IP_REGEX_PATTERN1 = @"IP_REGEX_PATTERN1";
            public const string IP_REGEX_PATTERN2 = @"IP_REGEX_PATTERN2";
            public const string LAUNCH_TIMEOUT_IN_SECONDS = @"LAUNCH_TIMEOUT_IN_SECONDS";
            public const string POPUP_BROWSER_ARGS = @"POPUP_BROWSER_ARGS";
            public const string POPUP_BROWSER_OR_URL = @"POPUP_BROWSER_OR_URL";
        }

        public void CoreForm_Load(object sender, EventArgs e)
        {
            LoadConfig(GetConfigPath());
            var main = new MainForm();
                main.Show();
        }

        public void LoadConfig(string cfgFile)
        {
            try
            {
                if (!File.Exists(cfgFile))
                {
                    MessageBox.Show(this, Properties.Strings.popupErrorCfgFileMissing + Environment.NewLine + Environment.NewLine + cfgFile + Environment.NewLine +
                                          Environment.NewLine + Properties.Strings.popupErrorBottomText,
                                          Properties.Strings.popupErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Environment.Exit(2);
                }
                
                using (StreamReader sr = new StreamReader(cfgFile))
                {
                    var cfg = new Dictionary<string, string>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line.Substring(0, 1).Equals('#'))
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
                        ctxClientArgs1 = cfg[CfgKeys.CTX_CLIENT_ARGS1];
                        ctxClientArgs2 = cfg[CfgKeys.CTX_CLIENT_ARGS2];
                        ctxClientPath = cfg[CfgKeys.CTX_CLIENT_PATH];
                        ctxWindowTitle = cfg[CfgKeys.CTX_WINDOW_TITLE];
                        ipRegexPattern1 = cfg[CfgKeys.IP_REGEX_PATTERN1];
                        ipRegexPattern2 = cfg[CfgKeys.IP_REGEX_PATTERN2];
                        launchTimeout = int.Parse(cfg[CfgKeys.LAUNCH_TIMEOUT_IN_SECONDS]);
                        popupBrowserArgs = cfg[CfgKeys.POPUP_BROWSER_ARGS];
                        popupBrowserOrURL = cfg[CfgKeys.POPUP_BROWSER_OR_URL];
                    }
                    else
                    {
                        throw new Exception(Properties.Strings.popupErrorCfgFileInvalid);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, Properties.Strings.popupErrorCfgFileNotReadable + Environment.NewLine + Environment.NewLine +
                                      e.Message + Environment.NewLine + Environment.NewLine + Properties.Strings.popupErrorBottomText,
                                      Properties.Strings.popupErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(1);
            }
        }

        private bool isConfigValid(Dictionary<string, string> cfg)
        {
            var cfgKeys = new CfgKeys();
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