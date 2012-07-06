using System;
using Common.Logging;

namespace Sekhmet.Serialization
{
    public class DefaultInstantiator : IInstantiator
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public virtual object Create(Type type, IAdviceRequester adviceRequester)
        {
            try
            {
                var instance = Activator.CreateInstance(type);

                if (_log.IsDebugEnabled)
                    _log.Debug("Created instance of '" + type + "': " + instance);

                return instance;
            }
            catch (MissingMethodException ex)
            {
                throw new MissingMethodException("No parameterless constructor defined for type '" + type + "'.", ex);
            }
        }
    }
}