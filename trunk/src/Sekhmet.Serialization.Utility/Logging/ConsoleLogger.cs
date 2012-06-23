using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public class ConsoleLogger : LoggerBase, ILogger
    {
        private readonly IConsoleWriter _writer;

        private bool _isDebugEnabled;
        private bool _isErrorEnabled;
        private bool _isFatalEnabled;
        private bool _isInfoEnabled;
        private bool _isTraceEnabled;
        private bool _isWarningEnabled;

        public override bool IsDebugEnabled
        {
            get { return _isDebugEnabled; }
        }

        public override bool IsErrorEnabled
        {
            get { return _isErrorEnabled; }
        }

        public override bool IsFatalEnabled
        {
            get { return _isFatalEnabled; }
        }

        public override bool IsInfoEnabled
        {
            get { return _isInfoEnabled; }
        }

        public override bool IsTraceEnabled
        {
            get { return _isTraceEnabled; }
        }

        public override bool IsWarningEnabled
        {
            get { return _isWarningEnabled; }
        }

        public string Name { get; private set; }

        public ConsoleLogger(string name, IConsoleWriter writer = null)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            _writer = writer ?? new DefaultConsoleWriter();
        }

        public override void Log(LogLevel level, string message, Exception exception = null)
        {
            if (!IsEnabled(level))
                return;

            ConsoleColor consoleColor;
            switch (level)
            {
                case LogLevel.Trace:
                    consoleColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.DarkGray;
                    break;
                case LogLevel.Info:
                    consoleColor = ConsoleColor.Black;
                    break;
                case LogLevel.Warning:
                    consoleColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    consoleColor = ConsoleColor.DarkRed;
                    break;
                case LogLevel.Fatal:
                    consoleColor = ConsoleColor.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }

            _writer.WriteLine(consoleColor, "[" + DateTime.Now.TimeOfDay + "] " + level.ToString().ToUpperInvariant() + ": " + message, exception);
        }

        public void SetLevel(LogLevel level)
        {
            _isTraceEnabled = level <= LogLevel.Trace;
            _isDebugEnabled = level <= LogLevel.Debug;
            _isInfoEnabled = level <= LogLevel.Info;
            _isWarningEnabled = level <= LogLevel.Warning;
            _isErrorEnabled = level <= LogLevel.Error;
            _isFatalEnabled = level <= LogLevel.Fatal;
        }
    }
}