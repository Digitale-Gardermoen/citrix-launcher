using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace citrix_launcher
{
    class CitrixLauncher : IConfigProvider
    {
        private ConfigurationLoader configLoader;
        private Configuration config;

        public Configuration GetConfiguration()
        {
            return config;
        }

        [STAThread]
        static void Main(string[] args)
        {
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

            var mainForm = new MainForm(this);
            InitializeApplication(mainForm, args);

            Application.Run(mainForm);
        }

        private void InitializeApplication(IErrorDisplayer errorDisplayer, string[] args)
        {
            if (args.Length == 2 && args[0].ToLower().Equals("-cfgpath") && File.Exists(args[1]) )
            {
                configLoader = new ConfigurationLoader(errorDisplayer, args[1]);
            }
            else
            {
                configLoader = new ConfigurationLoader(errorDisplayer);
            }

            config = configLoader.LoadConfig();
        }
    }
}
