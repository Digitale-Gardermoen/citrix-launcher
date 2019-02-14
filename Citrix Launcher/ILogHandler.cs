using System.Collections.Generic;

namespace citrix_launcher
{
    public interface ILogHandler
    {

        void Write(string text);

        void Write(Dictionary<string,string> dictionary);

        void Write(Configuration configuration);

        void SetLogLevel(LOGLEVEL loglevel);
    }

    public enum LOGLEVEL
    {
        NONE,
        ERROR,
        WARNING,
        INFO,
        DEBUG
    }

}