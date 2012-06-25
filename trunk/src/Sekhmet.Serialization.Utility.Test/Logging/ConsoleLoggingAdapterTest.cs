using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sekhmet.Serialization.Utility.Logging;
using Xunit;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class ConsoleLoggingAdapterTest
    {
        [Fact]
        public void TestGetLogger()
        {
            var adapter = new ConsoleLoggingAdapter();

            var logger = adapter.GetLogger();

            Assert.Equal(string.Empty, logger.Name);
        }

        [Fact]
        public void TestGetLogger_ForObject()
        {
            var adapter = new ConsoleLoggingAdapter();

            var foo = new Foo();
            var logger = adapter.GetLogger(foo);

            string expected = foo.ToString();

            Assert.Equal(expected, logger.Name);
        }

        [Fact]
        public void TestGetLogger_ForObject_Null()
        {
            var adapter = new ConsoleLoggingAdapter();

            Assert.Throws<ArgumentNullException>(() => adapter.GetLogger((object)null));
        }

        [Fact]
        public void TestGetLogger_ForString_LoggerIsReused()
        {
            var adapter = new ConsoleLoggingAdapter();

            var logger1 = adapter.GetLogger("foo");
            var logger2 = adapter.GetLogger("foo");

            Assert.Same(logger1, logger2);
        }

        [Fact]
        public void TestGetLogger_ForString_LoggerIsReused_Concurrent()
        {
            var adapter = new ConsoleLoggingAdapter();

            var loggers = new ConcurrentBag<ILogger>();
            var ewhs = new List<EventWaitHandle>();
            var globalEwh = new EventWaitHandle(false, EventResetMode.ManualReset);
            var threads = Enumerable.Range(0, 100)
                .Select(i =>
                {
                    var ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
                    ewhs.Add(ewh);

                    var thread = new Thread(() =>
                    {
                        ewh.Set();
                        globalEwh.WaitOne();
                        loggers.Add(adapter.GetLogger("foo"));
                    });
                    thread.Start();
                    return thread;
                })
                .ToArray();

            foreach (var ewh in ewhs)
                ewh.WaitOne();

            globalEwh.Set();

            foreach (var thread in threads)
                thread.Join();

            Assert.Equal(1, loggers.GroupBy(l => l).Count());
        }

        [Fact]
        public void TestGetLogger_ForString_Null()
        {
            var adapter = new ConsoleLoggingAdapter();

            Assert.Throws<ArgumentNullException>(() => adapter.GetLogger((string)null));
        }

        [Fact]
        public void TestGetLogger_ForType()
        {
            var adapter = new ConsoleLoggingAdapter();

            var logger = adapter.GetLogger(typeof(ConsoleLoggingAdapterTest));

            string expected = typeof(ConsoleLoggingAdapterTest).FullName;

            Assert.Equal(expected, logger.Name);
        }

        [Fact]
        public void TestGetLogger_ForType_Null()
        {
            var adapter = new ConsoleLoggingAdapter();

            Assert.Throws<ArgumentNullException>(() => adapter.GetLogger((Type)null));
        }

        [Fact]
        public void TestSetLevel_NewLoggers()
        {
            var adapter = new ConsoleLoggingAdapter();

            adapter.SetLevel("Foo.Bar.Baz", LogLevel.Info);

            var logger = adapter.GetLogger("Foo.Bar.Baz");

            Assert.False(logger.IsDebugEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsWarningEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);

            logger = adapter.GetLogger("Foo.Bar.Baz.Qut");

            Assert.False(logger.IsDebugEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsWarningEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);

            logger = adapter.GetLogger("Foo.Bar.Ba");

            Assert.False(logger.IsDebugEnabled);
            Assert.False(logger.IsInfoEnabled);
            Assert.False(logger.IsWarningEnabled);
            Assert.False(logger.IsErrorEnabled);
            Assert.False(logger.IsFatalEnabled);
        }

        [Fact]
        public void TestSetLevel_Existing()
        {
            var adapter = new ConsoleLoggingAdapter();

            var logger1 = adapter.GetLogger("Foo.Bar.Baz");

            Assert.False(logger1.IsDebugEnabled);
            Assert.False(logger1.IsInfoEnabled);
            Assert.False(logger1.IsWarningEnabled);
            Assert.False(logger1.IsErrorEnabled);
            Assert.False(logger1.IsFatalEnabled);

            var logger2 = adapter.GetLogger("Foo.Bar.Baz.Qut");

            Assert.False(logger2.IsDebugEnabled);
            Assert.False(logger2.IsInfoEnabled);
            Assert.False(logger2.IsWarningEnabled);
            Assert.False(logger2.IsErrorEnabled);
            Assert.False(logger2.IsFatalEnabled);

            adapter.SetLevel("Foo.Bar.Baz", LogLevel.Info);

            Assert.False(logger1.IsDebugEnabled);
            Assert.True(logger1.IsInfoEnabled);
            Assert.True(logger1.IsWarningEnabled);
            Assert.True(logger1.IsErrorEnabled);
            Assert.True(logger1.IsFatalEnabled);

            Assert.False(logger2.IsDebugEnabled);
            Assert.True(logger2.IsInfoEnabled);
            Assert.True(logger2.IsWarningEnabled);
            Assert.True(logger2.IsErrorEnabled);
            Assert.True(logger2.IsFatalEnabled);
        }

        private class Foo
        {
            public override string ToString()
            {
                return "MyFoo";
            }
        }
    }
}