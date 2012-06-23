using System;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization.Utility.Logging
{
    public class ConsoleLoggingAdapter : ILoggingAdapter
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
            using (_lock.EnterUpgradeableReadScope())
            {
                ConsoleLogger logger = null;
                WeakReference reference;
                if (_loggers.TryGetValue(name, out reference))
                    logger = reference.Target as ConsoleLogger;

                if (logger == null)
                {
                    using (_lock.EnterWriteScope())
                    {
                        if (_loggers.TryGetValue(name, out reference))
                            logger = reference.Target as ConsoleLogger;
                        else
                        {
                            logger = new ConsoleLogger(name);
                            _loggers[name] = new WeakReference(logger);
                        }
                    }
                }

                return logger;
            }
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

        private void SetLevels()
        {
            using (_lock.EnterReadScope())
            {
                foreach (var loggerPair in _loggers.ToList())
                {
                    var logger = loggerPair.Value.Target as ConsoleLogger;
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

                        logger.SetLevel(level);
                        break;
                    }
                }
            }
        }
    }
}