using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace citrix_launcher
{
    public partial class CoreForm : Form
    {
        string cfgPathParam;

        #region Form
        public CoreForm(string[] args)
        {
            if (Environment.UserName.ToLower().StartsWith("adm-")) {
                Environment.Exit(0);
            }

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
        public static string groupBasedConfig;
        public static string ldapServer;
        public static string ldapMemberOf;

        public string GetConfigPath()
        {
            if (cfgPathParam != null)
            {
                return cfgPathParam;
            }
            else
            {
                return @".\citrix-launcher.cfg";
            }
        }

        public struct MandatoryKeys
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

        public struct OptionalKeys
        {
            public const string GROUP_BASED_CONFIG = @"GROUP_BASED_CONFIG";
            public const string LDAP_SERVER = @"LDAP_SERVER";
            public const string LDAP_MEMBER_OF = @"LDAP_MEMBER_OF";
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

                var namespaces = new List<string>();
                var cfg = new Dictionary<string, string>();

                using (StreamReader sr = new StreamReader(cfgFile))
                {
                    string line;
                    string currentNamespace = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line.Substring(0, 1).Equals("#"))
                        {
                            continue;
                        }

                        if (Regex.IsMatch(line, @"^\[[a-z0-9\_]+\]$"))
                        {
                            currentNamespace = line.Substring(1, line.Length - 2);
                            namespaces.Add(currentNamespace);
                        }

                        GetKeyValuePair(line, cfg, currentNamespace);
                    }
                }

                var currentConfig = new Dictionary<string, string>();
                var @namespace = GetPrioritizedNamespace(namespaces, cfg);

                foreach(string nsKey in cfg.Keys) // TODO: LDAP oppslag
                {
                    var keyParts = nsKey.Split('.');

                    if (@namespace.Equals("global") || keyParts[0].Equals(@namespace))
                    {
                        currentConfig.Add(keyParts[1], cfg[nsKey]);
                    }
                }

                if (isConfigValid(currentConfig))
                {
                    ctxClientArgs1 = currentConfig[MandatoryKeys.CTX_CLIENT_ARGS1];
                    ctxClientArgs2 = currentConfig[MandatoryKeys.CTX_CLIENT_ARGS2];
                    ctxClientPath = currentConfig[MandatoryKeys.CTX_CLIENT_PATH];
                    ctxWindowTitle = currentConfig[MandatoryKeys.CTX_WINDOW_TITLE];
                    ipRegexPattern1 = currentConfig[MandatoryKeys.IP_REGEX_PATTERN1];
                    ipRegexPattern2 = currentConfig[MandatoryKeys.IP_REGEX_PATTERN2];
                    launchTimeout = int.Parse(currentConfig[MandatoryKeys.LAUNCH_TIMEOUT_IN_SECONDS]);
                    popupBrowserArgs = currentConfig[MandatoryKeys.POPUP_BROWSER_ARGS];
                    popupBrowserOrURL = currentConfig[MandatoryKeys.POPUP_BROWSER_OR_URL];
                    groupBasedConfig = currentConfig[OptionalKeys.GROUP_BASED_CONFIG];
                    ldapServer = currentConfig[OptionalKeys.LDAP_SERVER];
                    ldapMemberOf = currentConfig[OptionalKeys.LDAP_MEMBER_OF];
                }
                else
                {
                    throw new Exception(Properties.Strings.popupErrorCfgFileInvalid);
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

        private static string GetPrioritizedNamespace(List<string> namespaces, Dictionary<string, string> cfg)
        {
            string prioritizedNamespace = "";
            int highestPri = int.MaxValue;

            foreach (string ns in namespaces)
            {
                var key = ns + ".PRIORITY";

                if (cfg.ContainsKey(key))
                {
                    var pri = int.Parse(cfg[key]);

                    if (pri < highestPri)
                    {
                        highestPri = pri;
                        prioritizedNamespace = ns;
                    }
                }
            }

            return prioritizedNamespace;
        }

        private static void GetKeyValuePair(string line, Dictionary<string, string> cfg, string currentNamespace)
        {
            var lineParts = line.Split('=');
            var key = lineParts[0].Trim();

            if (currentNamespace != "")
            {
                key = currentNamespace + "." + key;
            }

            var value = lineParts[1].Trim();
            cfg.Add(key, value);
        }

        private bool isConfigValid(Dictionary<string, string> cfg)
        {
            var mandatoryKeys = new MandatoryKeys();
            var optionalKeys = new OptionalKeys();
            bool valid = true;

            foreach(var field in typeof(MandatoryKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(mandatoryKeys).ToString()))
                {
                    valid = false;
                }
            }

            foreach (var field in typeof(OptionalKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(optionalKeys).ToString()))
                {
                    cfg.Add(field.GetValue(optionalKeys).ToString(), "");
                }
            }

            return valid;
        }
    }
}