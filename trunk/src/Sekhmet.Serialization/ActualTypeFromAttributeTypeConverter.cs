using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class ActualTypeFromAttributeTypeConverter : ITypeConverter
    {
        public static readonly XName DefaultTypeAttributeName = "type";

        private readonly XName _attributeName;

        public ActualTypeFromAttributeTypeConverter(XName attributeName = null)
        {
            _attributeName = attributeName ?? DefaultTypeAttributeName;
        }

        public Type GetActualType(XObject source, IMemberContext target)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.NodeType != XmlNodeType.Element)
                return null;

            var elem = (XElement)source;

            var attr = elem.Attribute(_attributeName);
            if (attr == null)
                return null;

            return Type.GetType(attr.Value);
        }
    }
}