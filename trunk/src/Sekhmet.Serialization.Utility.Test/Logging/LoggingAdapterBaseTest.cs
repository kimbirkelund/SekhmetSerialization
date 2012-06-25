using System;
using Sekhmet.Serialization.Utility.Logging;
using Xunit;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class LoggingAdapterBaseTest
    {
        public static int DestructorCalls = 0;

        [Fact]
        public void TestGetLogger_GCed()
        {
            var adapter = new LoggingAdapterStub();
            adapter.SetLevel("Foo.Bar.Baz", LogLevel.Info);

            adapter.GetLogger("Foo.Bar.Baz");

            //while (DestructorCalls == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.Equal(1, DestructorCalls);

            Assert.NotNull(adapter.GetLogger("Foo.Bar.Baz"));
        }
    }

    public class LoggerStub : ILogger
    {
        public bool IsDebugEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsInfoEnabled { get; set; }
        public bool IsWarningEnabled { get; set; }
        public string Name { get; set; }

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

        ~LoggerStub()
        {
            LoggingAdapterBaseTest.DestructorCalls++;
        }
    }

    public class LoggingAdapterStub : LoggingAdapterBase<LoggerStub>
    {
        protected override LoggerStub CreateLogger(string name)
        {
            return new LoggerStub();
        }

        protected override void SetLevel(LoggerStub logger, LogLevel level) {}
    }
}