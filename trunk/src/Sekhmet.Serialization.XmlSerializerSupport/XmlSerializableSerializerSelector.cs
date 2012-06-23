using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializableSerializerSelector : ISerializerSelector
    {
        private readonly XmlSerializableSerializer _serializer = new XmlSerializableSerializer();

        public ISerializer Select(IMemberContext source, XObject target)
        {
            if (source == null)
                return null;
            if (target.NodeType != XmlNodeType.Element)
                return null;
            
            var sourceValue = source.GetValue();
            if (sourceValue == null)
                return null;
            
            var sourceValueObject = sourceValue.GetObject();
            if (sourceValueObject == null)
                return null;
            if (!sourceValueObject.GetType().IsSubTypeOf<IXmlSerializable>())
                return null;

            return _serializer;
        }
    }
}