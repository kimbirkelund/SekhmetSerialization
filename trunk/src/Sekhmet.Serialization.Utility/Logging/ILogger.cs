using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsTraceEnabled { get; }
        bool IsWarningEnabled { get; }

        void Debug(string message, Exception exception = null);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
        void Info(string message, Exception exception = null);
        bool IsEnabled(LogLevel level);
        void Log(LogLevel level, string message, Exception exception = null);
        void Trace(string message, Exception exception = null);
        void Warning(string message, Exception exception = null);
    }
}