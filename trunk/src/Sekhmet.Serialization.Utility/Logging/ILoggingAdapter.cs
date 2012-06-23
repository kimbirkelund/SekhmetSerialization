using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public interface ILoggingAdapter
    {
        ILogger GetLogger();
        ILogger GetLogger(Type type);
        ILogger GetLogger(string name);
        ILogger GetLogger(object instance);
    }
}