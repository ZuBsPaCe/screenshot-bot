using System;

namespace ScreenShotBot
{
    public interface ILog : IDisposable
    {
        void WriteDebug(string message);
        void WriteInfo(string message);
        void WriteWarning(string message);
        void WriteWarning(Exception ex);
        void WriteError(string message, Exception ex = null);
        void WriteError(Exception ex);
    }
}
