using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public abstract class LoggerBase
    {
        public abstract bool IsDebugEnabled { get; }
        public abstract bool IsErrorEnabled { get; }
        public abstract bool IsFatalEnabled { get; }
        public abstract bool IsInfoEnabled { get; }
        public abstract bool IsTraceEnabled { get; }
        public abstract bool IsWarningEnabled { get; }

        public virtual void Debug(string message, Exception exception = null)
        {
            Log(LogLevel.Debug, message, exception);
        }

        public virtual void Error(string message, Exception exception = null)
        {
            Log(LogLevel.Error, message, exception);
        }

        public virtual void Fatal(string message, Exception exception = null)
        {
            Log(LogLevel.Fatal, message, exception);
        }

        public virtual void Info(string message, Exception exception = null)
        {
            Log(LogLevel.Info, message, exception);
        }

        public virtual bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return IsTraceEnabled;
                case LogLevel.Debug:
                    return IsDebugEnabled;
                case LogLevel.Info:
                    return IsInfoEnabled;
                case LogLevel.Warning:
                    return IsWarningEnabled;
                case LogLevel.Error:
                    return IsErrorEnabled;
                case LogLevel.Fatal:
                    return IsFatalEnabled;
                default:
                    return false;
            }
        }

        public abstract void Log(LogLevel level, string message, Exception exception = null);

        public virtual void Trace(string message, Exception exception = null)
        {
            Log(LogLevel.Trace, message, exception);
        }

        public virtual void Warning(string message, Exception exception = null)
        {
            Log(LogLevel.Warning, message, exception);
        }
    }
}