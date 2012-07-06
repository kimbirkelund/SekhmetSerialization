using System;

namespace Sekhmet.Serialization
{
    public interface IInstantiator
    {
        object Create(Type type, IAdviceRequester adviceRequester);
    }
}