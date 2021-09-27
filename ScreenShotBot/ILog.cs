using System;

namespace ScreenShotBot
{
    public interface ILog : IDisposable
    {
        bool Tracing { get; set; }

        void WriteTrace(string message);
        void WriteDebug(string message);
        void WriteInfo(string message);
        void WriteWarning(string message);
        void WriteWarning(Exception ex);
        void WriteError(string message, Exception ex = null);
        void WriteError(Exception ex);
    }
}
