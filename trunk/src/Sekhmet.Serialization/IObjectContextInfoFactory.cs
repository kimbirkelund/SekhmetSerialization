using System;

namespace Sekhmet.Serialization
{
    public interface IObjectContextInfoFactory
    {
        ObjectContextInfo Create(IObjectContextFactory objectContextFactory, Type type);
    }
}