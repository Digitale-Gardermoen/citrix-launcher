using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;

namespace citrix_launcher
{
    public class ConfigurationLoader
    {
        static private string defaultConfigPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\citrix-launcher.cfg";

        private Configuration config;
        private IErrorDisplayer errorViewDelegate;
        private ILogHandler logger;
        private string cfgPath;

        private bool didMatchIp = false;

        public ConfigurationLoader(IErrorDisplayer errorDelegate, ILogHandler logger) : this(errorDelegate, logger, defaultConfigPath) { }

        public ConfigurationLoader(IErrorDisplayer errorDelegate, ILogHandler logger, string configPath)
        {
            this.errorViewDelegate = errorDelegate;
            this.cfgPath = configPath;
            this.logger = logger;
        }

        public Configuration LoadConfig()
        {
            config = new Configuration();
            try
            {
                if (!File.Exists(cfgPath))
                {
                    var exitcode = 2;

                    var msg = Properties.Strings.errorCfgFileMissing;
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

                var msg = Properties.Strings.errorCfgFileNotReadable;
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
                // TODO: Make this loading dynamic
                config.CitrixClearCache = bool.Parse(currentConfig[Configuration.MandatoryKeys.CITRIX_CLEAR_CACHE]);
                config.CitrixClientArgs = currentConfig[Configuration.MandatoryKeys.CITRIX_CLIENT_ARGS];
                config.CitrixClientPath = currentConfig[Configuration.MandatoryKeys.CITRIX_CLIENT_PATH];
                config.CitrixWindowTitle = currentConfig[Configuration.MandatoryKeys.CITRIX_WINDOW_TITLE];
                config.LaunchTimeout = int.Parse(currentConfig[Configuration.MandatoryKeys.LAUNCH_TIMEOUT_IN_SECONDS]);

                if (currentConfig.ContainsKey(Configuration.OptionalKeys.CITRIX_AUTOSTART))
                {
                    config.CitrixAutostart = bool.Parse(currentConfig[Configuration.OptionalKeys.CITRIX_AUTOSTART]);
                    config.CitrixCachePath = Environment.ExpandEnvironmentVariables(currentConfig[Configuration.OptionalKeys.CITRIX_CACHE_PATH]);
                }

                if (!File.Exists(config.CitrixClientPath))
                {
                    var exitcode = 2;

                    var msg = Properties.Strings.errorCtxClientMissing;
                    msg += Environment.NewLine;
                    msg += "[ " + config.CitrixClientPath + " ]";
                    Console.WriteLine(config.CitrixClientPath);

                    errorViewDelegate.ExitWithError(msg, exitcode);
                }
            }
            else if (IsFallbackConfigValid(currentConfig))
            {
                config.useFallbackConfig = true;
                config.BrowserURL = currentConfig[Configuration.MandatoryFallbackKeys.BROWSER_URL];
                config.BrowserPath = currentConfig[Configuration.MandatoryFallbackKeys.BROWSER_PATH];
            }
            else
            {
                throw new Exception(Properties.Strings.errorCfgFileInvalid);
            }
        }

        private Dictionary<string, string> ParseConfig(List<string> namespaces, Dictionary<string, string> cfg)
        {
            Dictionary<string, string> currentConfig = GetNamespacedConfig(namespaces, cfg);
            this.config.didMatchIp = this.didMatchIp;
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
                var domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;

                foreach (IPAddress adr in ipAddresses)
                {
                    string ipAddress = adr.ToString();

                    if (Regex.IsMatch(ipAddress, ipRegEx))
                    {
                        match = true;
                        logger.Write(string.Format("IP-address {0} matched pattern {1}", ipAddress, ipRegEx));
                        break;
                    }
                }

                if (match)
                {
                    this.didMatchIp = true;
                    ipMatchedNS = ns;
                    logger.Write(string.Format("IP matched namespace: " + ipMatchedNS));
                }
            }

            if (ipMatchedNS == "") return "";
 

            if (!DoGroupBasedConfig(cfg))
            {
                return ipMatchedNS;
            }

            var ldapKeyBase = ".LDAP_MEMBER_OF_REGEX_PATTERN";
            var prioKeyBase = ".PRIORITY";

            var prioritizedNamespace = "";
            var highestPri = int.MaxValue;

            try
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Environment.UserName);
                PrincipalSearchResult<Principal> groups;

                if (user!= null)
                {
                    groups = user.GetAuthorizationGroups();

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

                                    logger.Write(string.Format("Matched group {0} with pattern {1} and priority {2} from namespace {3}.", group.Name, ldapGroupPattern, pri, ns));
                                    logger.Write("Current highest priority: " + highestPri + " Current prioritized namespace: " + prioritizedNamespace);
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
            catch(Exception)
            {
                /* Don't do anything to failed connections to AD */
                logger.Write("Could not connect to Active Directory.");
            }

            if (prioritizedNamespace == "")
            {
                logger.Write(Properties.Strings.errorCfgFileNoGroupCfgOrLDAPMismatch);
                throw new Exception(Properties.Strings.errorCfgFileNoGroupCfgOrLDAPMismatch);
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

            logger.Write(string.Format("Performing group based configuration: {0}", doGroupBased));

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
            logger.Write(string.Format("Read from config file: {0} -> {1}", key, value));
        }

        private bool IsConfigValid(Dictionary<string, string> cfg)
        {
            var mandatoryKeys = new Configuration.MandatoryKeys();
            bool valid = true;

            foreach (var field in typeof(Configuration.MandatoryKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(mandatoryKeys).ToString()))
                {
                    logger.Write("Did not find mandatory field: " + field);
                    valid = false;
                }
            }

            return valid;
        }

        private bool IsFallbackConfigValid(Dictionary<string, string> cfg)
        {
            var mandatoryFallbackKeys = new Configuration.MandatoryFallbackKeys();
            bool valid = true;

            foreach (var field in typeof(Configuration.MandatoryFallbackKeys).GetFields())
            {
                if (!cfg.ContainsKey(field.GetValue(mandatoryFallbackKeys).ToString()))
                {
                    logger.Write("Did not find mandatory fallback field: " + field);
                    valid = false;
                }
            }

            return valid;
        }
    }
}