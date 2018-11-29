namespace citrix_launcher
{
    public class Configuration
    {
        // Mandatory Configuration Items
        public string BrowserPath { get; set; }
        public string BrowserURL { get; set; }
        public bool CtxClearCache { get; set; }
        public string CtxClientPath { get;  set; }

        private string _ctxClientArgs;
        public string CtxClientArgs
        {
            get => " " + _ctxClientArgs;
            set => _ctxClientArgs = value;
        }

        public string CtxWindowTitle { get;  set; }
        public string IpRegexPattern { get;  set; }
        public int LaunchTimeout { get;  set; }

        // Optional Configuration Items
        public bool CtxAutostart { get; set; } = false;
        public string CtxCachePath { get; set; } = @"%LOCALAPPDATA%\Citrix\SelfService";
        public string GroupBasedConfig { get;  set; } = "";
        public string LdapServer { get;  set; } = "";
        public string LdapMemberOf { get;  set; } = "";

        public struct MandatoryKeys
        {
            public const string BROWSER_URL = @"BROWSER_URL";
            public const string BROWSER_PATH = @"BROWSER_PATH";
            public const string CTX_CLEAR_CACHE = @"CTX_CLEAR_CACHE";
            public const string CTX_CLIENT_ARGS = @"CTX_CLIENT_ARGS";
            public const string CTX_CLIENT_PATH = @"CTX_CLIENT_PATH";
            public const string CTX_WINDOW_TITLE = @"CTX_WINDOW_TITLE";
            public const string LAUNCH_TIMEOUT_IN_SECONDS = @"LAUNCH_TIMEOUT_IN_SECONDS";
            public const string IP_REGEX_PATTERN = @"IP_REGEX_PATTERN";
        }

        public struct OptionalKeys
        {
            public const string CTX_AUTOSTART = @"CTX_AUTOSTART";
            public const string CTX_CACHE_PATH = @"CTX_CACHE_PATH";
            public const string GROUP_BASED_CONFIG = @"GROUP_BASED_CONFIG";
            public const string LDAP_MEMBER_OF = @"LDAP_MEMBER_OF";
        }
    }
}
