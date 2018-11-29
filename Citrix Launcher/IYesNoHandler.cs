using System.Windows.Forms;

namespace citrix_launcher
{
    public interface IYesNoHandler
    {
        void YesNoHandler(bool answeredYes, string prompt, Form form);
    }
}