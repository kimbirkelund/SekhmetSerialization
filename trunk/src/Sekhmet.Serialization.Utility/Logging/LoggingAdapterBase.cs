using System;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization.Utility.Logging
{
    public abstract class LoggingAdapterBase<TLogger> : ILoggingAdapter
        where TLogger : class, ILogger
    {
        private readonly ReadWriteLock _lock = new ReadWriteLock();
        private readonly IDictionary<string, LogLevel> _logLevels = new Dictionary<string, LogLevel>();
        private readonly IDictionary<string, WeakReference> _loggers = new Dictionary<string, WeakReference>();

        public ILogger GetLogger()
        {
            return GetLogger("");
        }

        public ILogger GetLogger(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return GetLogger(type.FullName);
        }

        public ILogger GetLogger(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            TLogger logger = null;
            WeakReference reference;
            using (_lock.EnterReadScope())
            {
                if (_loggers.TryGetValue(name, out reference))
                    logger = reference.Target as TLogger;
            }

            if (logger == null)
            {
                using (_lock.EnterWriteScope())
                {
                    if (_loggers.TryGetValue(name, out reference))
                        logger = reference.Target as TLogger;
                    else
                    {
                        logger = CreateLogger(name);
                        _loggers[name] = new WeakReference(logger);
                        SetLevels();
                    }
                }
            }

            return logger;
        }

        public ILogger GetLogger(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            return GetLogger(instance.ToString());
        }

        public void SetLevel(string loggerName, LogLevel level)
        {
            using (_lock.EnterWriteScope())
                _logLevels[loggerName] = level;

            SetLevels();
        }

        protected abstract TLogger CreateLogger(string name);
        protected abstract void SetLevel(TLogger logger, LogLevel level);

        protected virtual void SetLevels()
        {
            using (_lock.EnterReadScope())
            {
                foreach (var loggerPair in _loggers.ToList())
                {
                    var logger = loggerPair.Value.Target as TLogger;
                    if (logger == null)
                    {
                        _loggers.Remove(loggerPair.Key);
                        continue;
                    }

                    foreach (var logLevelPair in _logLevels.OrderByDescending(p => p.Key))
                    {
                        var loggerName = logLevelPair.Key;
                        var level = logLevelPair.Value;

                        if (!loggerPair.Key.StartsWith(loggerName))
                            continue;

                        SetLevel(logger, level);
                        break;
                    }
                }
            }
        }
    }
}