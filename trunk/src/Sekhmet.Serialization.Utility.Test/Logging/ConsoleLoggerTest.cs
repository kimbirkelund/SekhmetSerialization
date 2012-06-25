using System;
using NUnit.Framework;
using Sekhmet.Serialization.Utility.Logging;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    [TestFixture]
    public class ConsoleLoggerTest
    {
        [Test]
        public void TestConstructor_NullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsoleLogger(null));
        }

        [Test]
        public void TestDebug()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.AreEqual(0, writer.CallCount);

            logger.Debug("Bob", new Exception());

            Assert.AreEqual(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.AreEqual(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Debug);

            logger.Debug("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
            Assert.NotNull(writer.Message);
            Assert.IsTrue(writer.Message.IndexOf("] DEBUG: Bob", StringComparison.Ordinal) != -1);
            Assert.NotNull(writer.Exception);
            Assert.AreEqual(ConsoleColor.DarkGray, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Info);

            logger.Debug("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
        }

        [Test]
        public void TestError()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.AreEqual(0, writer.CallCount);

            logger.Error("Bob", new Exception());

            Assert.AreEqual(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.AreEqual(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Warning);

            logger.Error("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
            Assert.NotNull(writer.Message);
            Assert.IsTrue(writer.Message.IndexOf("] ERROR: Bob", StringComparison.Ordinal) != -1);
            Assert.NotNull(writer.Exception);
            Assert.AreEqual(ConsoleColor.DarkRed, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Fatal);

            logger.Error("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
        }

        [Test]
        public void TestFatal()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.AreEqual(0, writer.CallCount);

            logger.Fatal("Bob", new Exception());

            Assert.AreEqual(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.AreEqual(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Error);

            logger.Fatal("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
            Assert.NotNull(writer.Message);
            Assert.IsTrue(writer.Message.IndexOf("] FATAL: Bob", StringComparison.Ordinal) != -1);
            Assert.NotNull(writer.Exception);
            Assert.AreEqual(ConsoleColor.Red, writer.ConsoleColor);

            logger.Fatal("Bob", new Exception());

            Assert.AreEqual(2, writer.CallCount);
        }

        [Test]
        public void TestInfo()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.AreEqual(0, writer.CallCount);

            logger.Debug("Bob", new Exception());

            Assert.AreEqual(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.AreEqual(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Debug);

            logger.Info("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
            Assert.NotNull(writer.Message);
            Assert.IsTrue(writer.Message.IndexOf("] INFO: Bob", StringComparison.Ordinal) != -1);
            Assert.NotNull(writer.Exception);
            Assert.AreEqual(ConsoleColor.Black, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Warning);

            logger.Info("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
        }

        [Test]
        public void TestWarning()
        {
            var writer = new DummyConsoleWriter();
            var logger = new ConsoleLogger("boo", writer);

            Assert.AreEqual(0, writer.CallCount);

            logger.Warning("Bob", new Exception());

            Assert.AreEqual(0, writer.CallCount);
            Assert.Null(writer.Message);
            Assert.Null(writer.Exception);
            Assert.AreEqual(default(ConsoleColor), writer.ConsoleColor);

            logger.SetLevel(LogLevel.Info);

            logger.Warning("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
            Assert.NotNull(writer.Message);
            Assert.IsTrue(writer.Message.IndexOf("] WARNING: Bob", StringComparison.Ordinal) != -1);
            Assert.NotNull(writer.Exception);
            Assert.AreEqual(ConsoleColor.Yellow, writer.ConsoleColor);

            logger.SetLevel(LogLevel.Error);

            logger.Warning("Bob", new Exception());

            Assert.AreEqual(1, writer.CallCount);
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