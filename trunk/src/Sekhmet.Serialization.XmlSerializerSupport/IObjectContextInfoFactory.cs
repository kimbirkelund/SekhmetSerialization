using System;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public interface IObjectContextInfoFactory
    {
        ObjectContextInfo Create(IObjectContextFactory objectContextFactory, Type type);
    }
}