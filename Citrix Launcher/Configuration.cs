using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace citrix_launcher
{
    public class Configuration
    {
        // Mandatory Configuration Items
        public string CtxClientPath { get;  set; }

        private string _ctxClientArgs1;
        public string CtxClientArgs1
        {
            get => " " + _ctxClientArgs1;
            set => _ctxClientArgs1 = value;
        }

        private string _ctxClientArgs2;
        public string CtxClientArgs2
        {
            get => " " + _ctxClientArgs2;
            set => _ctxClientArgs2 = value;
        }

        public string CtxWindowTitle { get;  set; }
        public string IpRegexPattern1 { get;  set; }
        public string IpRegexPattern2 { get;  set; }
        public int LaunchTimeout { get;  set; }
        public string PopupBrowserOrURL { get;  set; }
        public string PopupBrowserArgs { get;  set; }

        // Optional Configuration Items
        public string GroupBasedConfig { get;  set; } = "";
        public string LdapServer { get;  set; } = "";
        public string LdapMemberOf { get;  set; } = "";

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
            public const string LDAP_MEMBER_OF = @"LDAP_MEMBER_OF";
        }
    }
}
