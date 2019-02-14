using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace citrix_launcher
{
    class CitrixLauncher : IConfigProvider
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private ConfigurationLoader configLoader;
        private Configuration config;
        private ILogHandler logger;

        public Configuration GetConfiguration()
        {
            return config;
        }

        [STAThread]
        static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            Mutex mutex = new Mutex(true, "{6136359e-fa7d-4739-9ad4-85cdbbb41b77}");
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                Environment.Exit(0);
            }

            var launcher = new CitrixLauncher(args);
            mutex.ReleaseMutex();
        }

        public CitrixLauncher(string[] args)
        {
            if (Environment.UserName.ToLower().StartsWith("adm-"))
            {
                Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            logger = new Logger();
            var mainForm = new MainForm(this, logger);
            InitializeApplication(mainForm, args);

            Application.Run(mainForm);
        }

        private void InitializeApplication(IErrorDisplayer errorDisplayer, string[] args)
        {
            var arguments = ParseArguments(args);

            foreach (var item in arguments)
            {
                switch(item.Key)
                {
                    case "cfgpath":
                        if (!File.Exists(item.Value))
                        {
                            Console.WriteLine("\r\nConfiguration file not found: " + item.Value);
                            Environment.Exit(404);
                        }
                        configLoader = new ConfigurationLoader(errorDisplayer, logger, item.Value);
                        break;
                    case "debug":
                        logger.SetLogLevel(LOGLEVEL.DEBUG);
                        break;
                }
            }

            if (configLoader == null)
            {
                configLoader = new ConfigurationLoader(errorDisplayer, logger);
            }

            config = configLoader.LoadConfig();
            logger.Write(config);
        }

        private static Dictionary<string,string> ParseArguments(string[] args)
        {
            var arguments = new Dictionary<string, string>();
            string[] validFlags = { "-help", "-cfgpath", "-debug" };

            for (int i = 0; i < args.Length; i++)
             {
                var arg = args[i];

                if ((arg[0] == '-' && validFlags.Contains(arg)) && arg != "-help")
                {
                    if (i == args.Length - 1 || args[i + 1][0] == '-')
                    {
                        arguments.Add(arg.Substring(1), "true");
                    }
                    else
                    {
                        arguments.Add(arg.Substring(1), args[i + 1]);
                        i++;
                    }
                }
                else
                {
                    var msg = "";

                    if (arg != "-help")
                    {
                        msg += "\r\n";
                        msg += "Invalid arguments supplied.\r\n";
                    }

                    msg += "\r\n";
                    msg += "Available arguments are:\r\n";
                    msg += "\r\n";
                    msg += "-help       Display this message\r\n";
                    msg += "-cfgpath    Custom path for loading the configuration file\r\n";
                    msg += "-debug      Enables logging to %LOCALAPPDATA%\r\n";
                    msg += "\r\n";
                    msg += "Example:\r\n";
                    msg += "\r\n";
                    msg += "Citrix Launcher.exe -cfgpath \"C:\\Users\\MyUser\\Documents\\my-custom-config.cfg\" -debug";

                    Console.WriteLine(msg);
                    Environment.Exit(1);
                }
            }

            return arguments;
        }
    }
}
