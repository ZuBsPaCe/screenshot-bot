using System;
using System.IO;

namespace ScreenShotBot
{
    public class LogFile : ILog
    {
        private readonly StreamWriter _stream;
        private readonly object _lock = new();

        public LogFile(string path, bool tracing)
        {
            _stream = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough));
            _stream.BaseStream.SetLength(0);
            _stream.AutoFlush = true;

            Tracing = tracing;
        }

        public bool Tracing { get; set; }

        public void WriteTrace(string message)
        {
            if (!Tracing)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff};TRACE;{message}");
            }
        }

        public void WriteDebug(string message)
        {
            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff};DEBUG;{message}");
            }
        }

        public void WriteInfo(string message)
        {
            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff};INFO;{message}");
            }
        }

        public void WriteWarning(string message)
        {
            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff};WARN;{message}");
            }
        }

        public void WriteWarning(Exception ex)
        {
            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff};WARN;{ex.Message}");
                _stream.WriteLine(ex.ToString());
            }
        }

        public void WriteError(string message, Exception ex = null)
        {
            if (ex == null)
            {
                lock (_lock)
                {
                    _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff};ERROR;{message}");
                }
            }
            else
            {
                lock (_lock)
                {
                    _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff};ERROR;{message}");
                    _stream.WriteLine(ex.ToString());
                }
            }
        }

        public void WriteError(Exception ex)
        {
            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff};RROR: {ex.Message}");
                _stream.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}