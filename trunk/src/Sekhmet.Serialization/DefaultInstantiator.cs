using System;

namespace Sekhmet.Serialization
{
    public class DefaultInstantiator : IInstantiator
    {
        public virtual object Create(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (MissingMethodException ex)
            {
                throw new MissingMethodException("No parameterless constructor defined for type '" + type + "'.", ex);
            }
        }
    }
}