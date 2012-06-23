using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class ValueTypeSerializer : ISerializer
    {
        public bool Serialize(IMemberContext source, XObject target)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            switch (target.NodeType)
            {
                case XmlNodeType.Element:
                    return SerializeToElement(source.GetValue(), (XElement)target, XmlSerializerHelper.IsNullable(source, (XElement)target));
                case XmlNodeType.Attribute:
                    return SerializeToAttribute(source.GetValue(), (XAttribute)target);
                default:
                    throw new ArgumentOutOfRangeException("source", "Unable to serialize to target of type '" + target.NodeType + "'.");
            }
        }

        private static bool SerializeToAttribute(IObjectContext source, XAttribute target)
        {
            var sourceObject = source != null ? source.GetObject() : null;

            if (sourceObject == null)
                return false;

            target.SetValue(sourceObject);
            return true;
        }

        private static bool SerializeToElement(IObjectContext source, XElement target, bool isNullable)
        {
            if (target.HasElements)
                throw new ArgumentException("Cannot serialize to an element which has child-elements.");

            var sourceObject = source != null ? source.GetObject() : null;

            if (sourceObject != null)
                target.SetValue(sourceObject);
            else if (isNullable)
                target.Add(Constants.XsiNilAttribute);
            else
                return false;

            return true;
        }
    }
}