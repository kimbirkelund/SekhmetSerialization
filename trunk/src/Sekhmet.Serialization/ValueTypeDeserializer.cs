using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class ValueTypeDeserializer : IDeserializer
    {
        private readonly ValueTypeDeserializerHelper _helper;
        private readonly Type _type;

        public ValueTypeDeserializer(Type type, ValueTypeDeserializerHelper helper = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            _type = type;
            _helper = helper ?? new ValueTypeDeserializerHelper();
        }

        public void Deserialize(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                return;

            IObjectContext targetObject;
            switch (source.NodeType)
            {
                case XmlNodeType.Element:
                    targetObject = DeserializeFromElement((XElement)source, _type);
                    break;
                case XmlNodeType.Attribute:
                    targetObject = DeserializeFromAttribute((XAttribute)source, _type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("source", "Unable to deserialize source of type '" + source.NodeType + "'.");
            }

            target.SetValue(targetObject);

            target.CommitChanges();
        }

        private IObjectContext DeserializeFromAttribute(XAttribute source, Type type)
        {
            return _helper.DeserializeFromValue(source.Value, type);
        }

        private IObjectContext DeserializeFromElement(XElement source, Type type)
        {
            if (source.HasElements)
                throw new ArgumentException("Cannot deserialize an element which has child-elements.");

            if (HasXsiNilTrueAttribute(source))
                return null;

            return _helper.DeserializeFromValue(source.Value, type);
        }

        private static bool HasXsiNilTrueAttribute(XElement source)
        {
            XAttribute attr = source.Attribute(Constants.XsiNilAttribute.Name);
            if (attr == null)
                return false;

            bool value;
            bool.TryParse(attr.Value, out value);

            return value;
        }
    }
}