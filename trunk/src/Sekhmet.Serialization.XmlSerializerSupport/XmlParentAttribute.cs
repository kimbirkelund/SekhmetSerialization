using System;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class XmlParentAttribute : Attribute { }
}