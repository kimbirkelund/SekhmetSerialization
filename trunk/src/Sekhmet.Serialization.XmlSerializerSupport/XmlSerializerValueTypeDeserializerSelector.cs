using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerValueTypeDeserializerSelector : ValueTypeDeserializerSelector
    {
        public override IDeserializer Select(XObject source, IMemberContext target)
        {
            var contractTypeFromAttr = GetContractTypeFromAttributes(source, target);
            if (contractTypeFromAttr == null)
                return null;

            return Select(source, target, contractTypeFromAttr);
        }

        private static Type GetContractTypeFromAttributes(XObject source, IMemberContext target)
        {
            return GetContractTypeFromXmlElementAttributes(source, target)
                   ?? GetContractTypeFromXmlArrayItemAttributes(source, target)
                   ?? GetContractTypeFromXmlAttributeAttributes(source, target);
        }

        private static Type GetContractTypeFromXmlArrayItemAttributes(XObject source, IMemberContext target)
        {
            var elem = source as XElement;
            if (elem == null)
                return null;

            return (from attr in target.Attributes.OfType<XmlArrayItemAttribute>()
                    where attr.Type != null
                    where attr.ElementName == elem.Name
                    select attr.Type).FirstOrDefault();
        }

        private static Type GetContractTypeFromXmlAttributeAttributes(XObject source, IMemberContext target)
        {
            var elem = source as XAttribute;
            if (elem == null)
                return null;

            return (from attr in target.Attributes.OfType<XmlAttributeAttribute>()
                    where attr.Type != null
                    where attr.AttributeName == elem.Name
                    select attr.Type).FirstOrDefault();
        }

        private static Type GetContractTypeFromXmlElementAttributes(XObject source, IMemberContext target)
        {
            var elem = source as XElement;
            if (elem == null)
                return null;

            return (from attr in target.Attributes.OfType<XmlElementAttribute>()
                    where attr.Type != null
                    where attr.ElementName == elem.Name
                    select attr.Type).FirstOrDefault();
        }
    }
}