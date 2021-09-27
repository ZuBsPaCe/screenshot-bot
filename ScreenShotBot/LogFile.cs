using System;
using System.IO;

namespace ScreenShotBot
{
    public class LogFile : ILog
    {
        private readonly StreamWriter _stream;
        private readonly object _lock = new();
        private readonly LogLevel _logLevel;

        public LogFile(string path, LogLevel logLevel)
        {
            _logLevel = logLevel;

            _stream = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));
            _stream.BaseStream.SetLength(0);
            _stream.AutoFlush = true;
        }

        public void WriteDebug(string message)
        {
            if (_logLevel < LogLevel.Debug)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} DEBUG: {message}");
            }
        }

        public void WriteInfo(string message)
        {
            if (_logLevel < LogLevel.Info)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} INFO : {message}");
            }
        }

        public void WriteWarning(string message)
        {
            if (_logLevel < LogLevel.Warn)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WARN : {message}");
            }
        }

        public void WriteWarning(Exception ex)
        {
            if (_logLevel < LogLevel.Warn)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff} WARN: {ex.Message}");
                _stream.WriteLine(ex.ToString());
            }
        }

        public void WriteError(string message, Exception ex = null)
        {
            if (_logLevel < LogLevel.Error)
            {
                return;
            }

            if (ex == null)
            {
                lock (_lock)
                {
                    _stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} ERROR: {message}");
                }
            }
            else
            {
                lock (_lock)
                {
                    _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff} ERROR: {message}");
                    _stream.WriteLine(ex.ToString());
                }
            }
        }

        public void WriteError(Exception ex)
        {
            if (_logLevel < LogLevel.Error)
            {
                return;
            }

            lock (_lock)
            {
                _stream.WriteLine($"{DateTime.Now:yyyy-MM-ddx HH:mm:ss.fff} ERROR: {ex.Message}");
                _stream.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}