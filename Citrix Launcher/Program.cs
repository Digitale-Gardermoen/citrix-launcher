using System;
using System.Threading;
using System.Windows.Forms;

namespace citrix_launcher
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{6136359e-fa7d-4739-9ad4-85cdbbb41b77}");
        [STAThread]
        static void Main(string[] args)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CoreForm(args));
                mutex.ReleaseMutex();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
