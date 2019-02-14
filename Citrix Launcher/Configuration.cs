namespace citrix_launcher
{
    public class Configuration
    {
        public bool useFallbackConfig = false;
        public bool didMatchIp = false;

        // Mandatory Configuration Items
        public bool CitrixClearCache { get; set; }
        public string CitrixClientPath { get;  set; }

        private string _ctxClientArgs;
        public string CitrixClientArgs
        {
            get => " " + _ctxClientArgs;
            set => _ctxClientArgs = value;
        }

        public string CitrixWindowTitle { get; set; }
        public int LaunchTimeout { get;  set; }

        // Optional Configuration Items
        public bool CitrixAutostart { get; set; } = false;
        public string CitrixCachePath { get; set; } = @"%LOCALAPPDATA%\Citrix\SelfService";

        // Mandatory Fallback Items
        public string BrowserPath { get; set; } = "";
        public string BrowserURL { get; set; } = "";

        public struct MandatoryKeys
        {
            public const string CITRIX_CLEAR_CACHE = @"CITRIX_CLEAR_CACHE";
            public const string CITRIX_CLIENT_ARGS = @"CITRIX_CLIENT_ARGS";
            public const string CITRIX_CLIENT_PATH = @"CITRIX_CLIENT_PATH";
            public const string CITRIX_WINDOW_TITLE = @"CITRIX_WINDOW_TITLE";
            public const string LAUNCH_TIMEOUT_IN_SECONDS = @"LAUNCH_TIMEOUT_IN_SECONDS";
        }

        public struct OptionalKeys
        {
            public const string CITRIX_AUTOSTART = @"CITRIX_AUTOSTART";
            public const string CITRIX_CACHE_PATH = @"CITRIX_CACHE_PATH";
            public const string GROUP_BASED_CONFIG = @"GROUP_BASED_CONFIG";
            public const string LDAP_MEMBER_OF_REGEX_PATTERN = @"LDAP_MEMBER_OF_REGEX_PATTERN";
        }

        public struct MandatoryFallbackKeys
        {
            public const string BROWSER_URL = @"BROWSER_URL";
            public const string BROWSER_PATH = @"BROWSER_PATH";
        }

        public override string ToString ()
        {
            var buffer = "";

            buffer += "- INTERNAL KEYS - \r\n";
            buffer += "useFallbackConfig: " + useFallbackConfig + "\r\n";
            buffer += "didMatchIp: " + didMatchIp + "\r\n";
            buffer += "\r\n";
            buffer += "- MANDATORY KEYS - \r\n";
            buffer += "CitrixClearCache: " + CitrixClearCache + "\r\n";
            buffer += "CitrixClientPath: " + CitrixClientPath + "\r\n";
            buffer += "CitrixClientArgs: " + CitrixClientArgs + "\r\n";
            buffer += "CitrixWindowTitle: " + CitrixWindowTitle + "\r\n";
            buffer += "LaunchTimeout: " + LaunchTimeout + "\r\n";
            buffer += "\r\n";
            buffer += "- OPTIONAL KEYS - \r\n";
            buffer += "CitrixAutostart: " + CitrixAutostart + "\r\n";
            buffer += "CitrixCachePath: " + CitrixCachePath + "\r\n";
            buffer += "\r\n";
            buffer += "- MANDATORY FALLBACK KEYS - \r\n";
            buffer += "BrowserPath: " + BrowserPath + "\r\n";
            buffer += "BrowserURL: " + BrowserURL;

            return buffer;
        }
    }
}
