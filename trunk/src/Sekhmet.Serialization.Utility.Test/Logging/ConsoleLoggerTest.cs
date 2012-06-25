using System;
using Sekhmet.Serialization.Utility.Logging;
using Xunit;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class ConsoleLoggerTest
    {
        [Fact]
        public void TestConstructor_NullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsoleLogger(null));
        }

        [Fact]
        public void TestDebug()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.Equal(0, writer.CallCount);

            logger.Debug("Bob", new Exception());

            Assert.Equal(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.Equal(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Debug);

            logger.Debug("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
            Assert.Contains("] DEBUG: Bob", writer.Message);
            Assert.NotNull(writer.Exception);
            Assert.Equal(ConsoleColor.DarkGray, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Info);

            logger.Debug("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
        }

        [Fact]
        public void TestError()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.Equal(0, writer.CallCount);

            logger.Error("Bob", new Exception());

            Assert.Equal(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.Equal(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Warning);

            logger.Error("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
            Assert.Contains("] ERROR: Bob", writer.Message);
            Assert.NotNull(writer.Exception);
            Assert.Equal(ConsoleColor.DarkRed, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Fatal);

            logger.Error("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
        }

        [Fact]
        public void TestFatal()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.Equal(0, writer.CallCount);

            logger.Fatal("Bob", new Exception());

            Assert.Equal(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.Equal(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Error);

            logger.Fatal("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
            Assert.Contains("] FATAL: Bob", writer.Message);
            Assert.NotNull(writer.Exception);
            Assert.Equal(ConsoleColor.Red, writer.ConsoleColor);

            logger.Fatal("Bob", new Exception());

            Assert.Equal(2, writer.CallCount);
        }

        [Fact]
        public void TestInfo()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.Equal(0, writer.CallCount);

            logger.Debug("Bob", new Exception());

            Assert.Equal(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.Equal(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Debug);

            logger.Info("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
            Assert.Contains("] INFO: Bob", writer.Message);
            Assert.NotNull(writer.Exception);
            Assert.Equal(ConsoleColor.Black, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Warning);

            logger.Info("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
        }

        [Fact]
        public void TestWarning()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.Equal(0, writer.CallCount);

            logger.Warning("Bob", new Exception());

            Assert.Equal(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.Equal(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Info);

            logger.Warning("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
            Assert.Contains("] WARNING: Bob", writer.Message);
            Assert.NotNull(writer.Exception);
            Assert.Equal(ConsoleColor.Yellow, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Error);

            logger.Warning("Bob", new Exception());

            Assert.Equal(1, writer.CallCount);
        }

        private class DummyConsoleWriter : IConsoleWriter
        {
            public int CallCount { get; private set; }
            public ConsoleColor ConsoleColor { get; private set; }
            public Exception Exception { get; private set; }
            public string Message { get; private set; }

            public void WriteLine(ConsoleColor consoleColor, string message, Exception exception)
            {
                ConsoleColor = consoleColor;
                Message = message;
                Exception = exception;
                CallCount++;
            }
        }
    }
}