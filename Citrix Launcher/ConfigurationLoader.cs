using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace citrix_launcher
{
    public class ConfigurationLoader
    {
        static private string defaultConfigPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\citrix-launcher.cfg";

        private Configuration config;
        private IErrorDisplayer errorViewDelegate;
        private string cfgPath;

        public ConfigurationLoader(IErrorDisplayer errorDelegate) : this(errorDelegate, defaultConfigPath) { }

        public ConfigurationLoader(IErrorDisplayer errorDelegate, string configPath)
        {
            this.errorViewDelegate = errorDelegate;
            this.cfgPath = configPath;
        }

        public Configuration LoadConfig()
        {
            config = new Configuration();
            try
            {
                if (!File.Exists(cfgPath))
                {
                    var exitcode = 2;

                    var msg = Properties.Strings.popupErrorCfgFileMissing;
                    msg += Environment.NewLine;
                    msg += "[ " + cfgPath + " ]";

                    errorViewDelegate.ExitWithError(msg, exitcode);
                }

                var namespaces = new List<string>();
                var cfg = new Dictionary<string, string>();
                ReadConfig(cfgPath, namespaces, cfg);

                var currentConfig = ParseConfig(namespaces, cfg);

                ValidateConfig(currentConfig);
            }
            catch (Exception e)
            {
                var exitcode = 1;

                var msg = Properties.Strings.popupErrorCfgFileNotReadable;
                msg += Environment.NewLine;
                msg += Environment.NewLine;
                msg += e.Message;

                errorViewDelegate.ExitWithError(msg, exitcode);
            }

            return config;
        }
        
        private void ValidateConfig(Dictionary<string, string> currentConfig)
        {
            if (IsConfigValid(currentConfig))
            {
                config.CtxClientArgs = currentConfig[Configuration.MandatoryKeys.CTX_CLIENT_ARGS];
                config.CtxClientPath = currentConfig[Configuration.MandatoryKeys.CTX_CLIENT_PATH];
                config.CtxWindowTitle = currentConfig[Configuration.MandatoryKeys.CTX_WINDOW_TITLE];
                config.IpRegexPattern = currentConfig[Configuration.MandatoryKeys.IP_REGEX_PATTERN];
                config.LaunchTimeout = int.Parse(currentConfig[Configuration.MandatoryKeys.LAUNCH_TIMEOUT_IN_SECONDS]);
                config.PopupBrowserArgs = currentConfig[Configuration.MandatoryKeys.POPUP_BROWSER_ARGS];
                config.PopupBrowserOrURL = currentConfig[Configuration.MandatoryKeys.POPUP_BROWSER_OR_URL];

                if (currentConfig.ContainsKey(Configuration.OptionalKeys.CTX_AUTOSTART))
                {
                    config.CtxAutostart = bool.Parse(currentConfig[Configuration.OptionalKeys.CTX_AUTOSTART]);
                }

                if (!File.Exists(config.CtxClientPath))
                {
                    var exitcode = 2;

                    var msg = Properties.Strings.popupErrorCtxClientMissing;
                    msg += Environment.NewLine;
                    msg += "[ " + config.CtxClientPath + " ]";
                    Console.WriteLine(config.CtxClientPath);

                    errorViewDelegate.ExitWithError(msg, exitcode);
                }
            }
            else
            {
                throw new Exception(Properties.Strings.popupErrorCfgFileInvalid);
            }
        }

        private Dictionary<string, string> ParseConfig(List<string> namespaces, Dictionary<string, string> cfg)
        {
            Dictionary<string, string> currentConfig = GetNamespacedConfig(namespaces, cfg);

            return currentConfig;
        }

        private Dictionary<string, string> GetNamespacedConfig(List<string> namespaces, Dictionary<string, string> cfg)
        {
            Dictionary<string, string> currentConfig = new Dictionary<string, string>();
            var @namespace = GetPrioritizedNamespace(namespaces, cfg);
            var nsParent = @namespace.Split('.')[0];

            foreach (string nsKey in cfg.Keys) 
            {
                var keyParts = nsKey.Split('.');
                var ns = keyParts[0];
                ns += keyParts.Length > 2 ? "." + keyParts[1] : "";

                var key = keyParts.Length > 2 ? keyParts[2] : keyParts[1];

                if (ns.Equals("global") && !currentConfig.ContainsKey(key))
                {
                    currentConfig[key] = cfg[nsKey];
                }

                if (ns.Equals(nsParent) && !currentConfig.ContainsKey(key))
                {
                    currentConfig[key] = cfg[nsKey];
                }

                if (ns.Equals(@namespace))
                {
                    currentConfig[key] = cfg[nsKey];
                }
            }

            return currentConfig;
        }

        private void ReadConfig(string cfgFile, List<string> namespaces, Dictionary<string, string> cfg)
        {
            using (StreamReader sr = new StreamReader(cfgFile))
            {
                string line;
                string currentNamespace = "";
                string subNamespace = "";
                string ns = "";

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0 || line.Substring(0, 1).Equals("#"))
                    {
                        continue;
                    }

                    if (Regex.IsMatch(line.ToLower(), @"^\[[a-z0-9_]+\]$"))
                    {
                        currentNamespace = line.Substring(1, line.Length - 2).ToLower();
                        subNamespace = "";
                        namespaces.Add(currentNamespace);
                        continue;
                    }

                    if (Regex.IsMatch(line.ToLower(), @"^\[\[[a-z0-9_]+\]\]$"))
                    {
                        subNamespace = line.Substring(2, line.Length - 4).ToLower();
                        namespaces.Add(currentNamespace + "." + subNamespace);
                        continue;
                    }

                    ns = subNamespace.Length > 0 ? currentNamespace + "." + subNamespace : currentNamespace;
                    GetKeyValuePair(line, cfg, ns);
                }
            }
        }

        private string GetPrioritizedNamespace(List<string> namespaces, Dictionary<string, string> cfg)
        {
            var ipRegexKeyBase = ".IP_REGEX_PATTERN";
            var ipMatchedNS = "";

            foreach (var ns in namespaces)
            {
                if (ns == "global") continue;
                if (ns.Contains(".")) continue;
                if (!cfg.ContainsKey(ns + ipRegexKeyBase)) continue;

                var ipRegEx = cfg[ns + ipRegexKeyBase];
                var ipAddresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                var match = false;
                foreach (IPAddress adr in ipAddresses)
                {
                    string ipAddress = adr.ToString();

                    if (Regex.IsMatch(ipAddress, ipRegEx))
                    {
                        match = true;
                        break;
                    }
                }

                if (match)
                {
                    ipMatchedNS = ns;
                }
            }

            if (ipMatchedNS == "") return "";

            if (!DoGroupBasedConfig(cfg))
            {
                return ipMatchedNS;
            }

            var ldapKeyBase = ".LDAP_MEMBER_OF";
            var prioKeyBase = ".PRIORITY";

            var prioritizedNamespace = "";
            var highestPri = int.MaxValue;

            try
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                var user = UserPrincipal.FindByIdentity(ctx, Environment.UserName);
                PrincipalSearchResult<Principal> groups;
                if (user!= null)
                {
                    groups = user.GetGroups();

                    foreach (var ns in namespaces)
                    {
                        if (ns == "global") continue;
                        if (!ns.Contains(ipMatchedNS)) continue;
                        if (cfg.ContainsKey(ns + ldapKeyBase))
                        {
                            var ldapGroupPattern = cfg[ns + ldapKeyBase];
                            foreach (var group in groups)
                            {
                                if (Regex.IsMatch(group.Name, ldapGroupPattern))
                                {
                                    var pri = int.Parse(cfg[ns + prioKeyBase]);
                                    if (pri < highestPri)
                                    {
                                        highestPri = pri;
                                        prioritizedNamespace = ns;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(ns + ldapKeyBase);
                        }
                    }
                }
            }
            catch(Exception){ /* Don't do anything to failed connections to AD */ }

            if (prioritizedNamespace == "")
            {
                throw new Exception(Properties.Strings.popupErrorCfgFileNoGroupConfig);
            }

            return prioritizedNamespace;
        }

        private bool DoGroupBasedConfig(Dictionary<string, string> cfg)
        {
            var doGroupBased = false;
            try
            {
                if (cfg.ContainsKey("global.GROUP_BASED_CONFIG"))
                {
                    doGroupBased = bool.Parse(cfg["global.GROUP_BASED_CONFIG"]);
                }
                else if (cfg.ContainsKey("GROUP_BASED_CONFIG"))
                {
                    doGroupBased = bool.Parse(cfg["GROUP_BASED_CONFIG"]);
                }
            }
            catch(Exception) { }

            return doGroupBased;
        }

        private void GetKeyValuePair(string line, Dictionary<string, string> cfg, string currentNamespace)
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

        private bool IsConfigValid(Dictionary<string, string> cfg)
        {
            var mandatoryKeys = new Configuration.MandatoryKeys();
            bool valid = true;

            foreach (var field in typeof(Configuration.MandatoryKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(mandatoryKeys).ToString()))
                {
                    valid = false;
                }
            }

            return valid;
        }
    }
}
