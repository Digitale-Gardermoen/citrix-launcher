using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace citrix_launcher
{
    class Logger : ILogHandler
    {
        LOGLEVEL loglevel = LOGLEVEL.NONE;
        StreamWriter writer;

        string logFilePath = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\");
        string logFileName = "citrix-launcher.log";

        public Logger()
        {
            LogRotate();

            writer = File.AppendText(logFilePath + logFileName);
            var separator = new string('*', 80);
            var startingString = string.Format("Starting application {0}", Process.GetCurrentProcess().ProcessName);
            var padLength = (int)Math.Floor((separator.Length - startingString.Length) / 2.0);
            var pad = new string(' ', padLength);

            loglevel = LOGLEVEL.INFO;

            this.Write(separator);
            this.Write(pad + startingString);
            this.Write(separator);

            loglevel = LOGLEVEL.NONE;
        }

        private void LogRotate()
        {
            if (!File.Exists(logFilePath + logFileName))
            {
                return;
            }

            var fileInfo = new FileInfo(logFilePath + logFileName);
            var len = fileInfo.Length;

            if (len <= 1024 * 500)
            {
                return;
            }

            string[] logs = Directory.GetFiles(logFilePath, logFileName + ".*");

            for (var i = logs.Length - 1; i >= 0; i--)
            {
                var nameParts = logs[i].Split('.');
                if (nameParts.Length < 3) continue;
                var logNum = int.Parse(nameParts[nameParts.Length - 1]);

                if (logNum >= 7)
                {
                    File.Delete(logs[i]);
                    continue;
                }

                logNum++;

                var oldPath = logs[i];
                var newPath = logFilePath + logFileName + "." + logNum;

                try
                {
                    File.Move(oldPath, newPath);
                }
                catch (System.NotSupportedException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            File.Move(logFilePath + logFileName, logFilePath + logFileName + ".0");
        }

        public void SetLogLevel(LOGLEVEL loglevel)
        {
            this.loglevel = loglevel;
        }

        public void Write(string text)
        {
            if (loglevel == LOGLEVEL.NONE)
            {
                return;
            }

            var now = DateTime.Now;
            var nowString = string.Format("{0} {1}: ", now.ToShortDateString(), now.ToString("HH:mm:ss"));

            if (text.Contains("\r\n"))
            {
                var lines = text.Split(new[] { "\r\n" }, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    writer.WriteLine(nowString + line);
                    writer.Flush();
                }
            }
            else
            {
                writer.WriteLine(nowString + text);
                writer.Flush();
            }
        }

        public void Write(string text, LOGLEVEL loglevel)
        {
            if (loglevel <= this.loglevel)
            {
                this.Write(text);
            }
        }

        public void Write(Dictionary<string, string> dictionary)
        {
            string buffer = "";

            foreach (var kvp in dictionary)
            {
                buffer += string.Format("\t{0} : {1}", kvp.Key, kvp.Value);
            }

            this.Write(buffer);
        }

        public void Write(Configuration configuration)
        {
            this.Write(Environment.NewLine + "Active configuration:" + Environment.NewLine);
            this.Write(configuration.ToString());
        }
    }
}
