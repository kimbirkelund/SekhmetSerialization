using Sekhmet.Serialization.Utility.Logging;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class LoggingAdapterStub : LoggingAdapterBase<LoggerStub>
    {
        protected override LoggerStub CreateLogger(string name)
        {
            return new LoggerStub();
        }

        protected override void SetLevel(LoggerStub logger, LogLevel level) {}
    }
}