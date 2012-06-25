using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization.Test
{
    public class SerializationManagerStub : ISerializationManager
    {
        public int DeserializeCallCount { get; private set; }
        public XElement DeserializeSource { get; private set; }
        public Type DeserializeTargetType { get; private set; }
        public int SerializeCallCount { get; private set; }
        public object SerializeSource { get; private set; }

        public object Deserialize(XElement source, Type targetType)
        {
            DeserializeSource = source;
            DeserializeTargetType = targetType;
            DeserializeCallCount++;

            return Activator.CreateInstance(targetType);
        }

        public XElement Serialize(object source)
        {
            SerializeSource = source;
            SerializeCallCount++;

            return new XElement("element");
        }
    }
}