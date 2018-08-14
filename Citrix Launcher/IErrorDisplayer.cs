namespace citrix_launcher
{
    public interface IErrorDisplayer
    {
        void ExitWithError(string msg, int exitcode);
    }
}