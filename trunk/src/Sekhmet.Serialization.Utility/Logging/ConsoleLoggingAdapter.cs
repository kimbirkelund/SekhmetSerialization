namespace Sekhmet.Serialization.Utility.Logging
{
    public class ConsoleLoggingAdapter : LoggingAdapterBase<ConsoleLogger>
    {
        protected override ConsoleLogger CreateLogger(string name)
        {
            return new ConsoleLogger(name);
        }

        protected override void SetLevel(ConsoleLogger logger, LogLevel level)
        {
            logger.SetLevel(level);
        }
    }
}