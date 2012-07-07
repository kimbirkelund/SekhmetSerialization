using System;
using System.Xml;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class ValueTypeSerializer : ISerializer
    {
        private readonly IIsNullableStrategy _isNullableStrategy;

        public ValueTypeSerializer(IIsNullableStrategy isNullableStrategy = null)
        {
            _isNullableStrategy = isNullableStrategy ?? new DefaultIsNullableStrategy();
        }

        public bool Serialize(IMemberContext source, XObject target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            switch (target.NodeType)
            {
                case XmlNodeType.Element:
                    return SerializeToElement(source.GetValue(), (XElement)target, _isNullableStrategy.IsNullable(source, (XElement)target, adviceRequester));
                case XmlNodeType.Attribute:
                    return SerializeToAttribute(source.GetValue(), (XAttribute)target);
                default:
                    throw new ArgumentOutOfRangeException("source", "Unable to serialize to target of type '" + target.NodeType + "'.");
            }
        }

        private static bool SerializeToAttribute(IObjectContext source, XAttribute target)
        {
            object sourceObject = source != null ? source.GetObject() : null;

            if (sourceObject == null)
                return false;

            target.SetValue(sourceObject);
            return true;
        }

        private static bool SerializeToElement(IObjectContext source, XElement target, bool isNullable)
        {
            if (target.HasElements)
                throw new ArgumentException("Cannot serialize to an element which has child-elements.");

            object sourceObject = source != null ? source.GetObject() : null;

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