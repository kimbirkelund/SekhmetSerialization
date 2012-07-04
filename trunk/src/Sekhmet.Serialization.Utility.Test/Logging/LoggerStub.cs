using System;
using Sekhmet.Serialization.Utility.Logging;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class LoggerStub : ILogger
    {
        public bool IsDebugEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsInfoEnabled { get; set; }
        public bool IsWarningEnabled { get; set; }
        public string Name { get; set; }

        public LoggerStub()
        {
            LoggingAdapterBaseTest.ConstructorCalls++;
        }

        ~LoggerStub()
        {
            LoggingAdapterBaseTest.DestructorCalls++;
        }

        public void Debug(string message, Exception exception = null) {}

        public void Error(string message, Exception exception = null) {}

        public void Fatal(string message, Exception exception = null) {}

        public void Info(string message, Exception exception = null) {}

        public bool IsEnabled(LogLevel level)
        {
            return false;
        }

        public void Log(LogLevel level, string message, Exception exception = null) {}

        public void Warning(string message, Exception exception = null) {}
    }
}