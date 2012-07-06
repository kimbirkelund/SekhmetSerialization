using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerTypeConverter : ITypeConverter
    {
        public Type GetActualType(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            return GetActualTypeFromXmlAttributeAttribute(source as XAttribute, target)
                   ?? GetActualTypeFromXmlElementAttribute(source as XElement, target)
                   ?? GetActualTypeFromXmlArrayItemAttribute(source as XElement, target);
        }

        private static Type GetActualTypeFromXmlArrayItemAttribute(XElement source, IMemberContext target)
        {
            if (source == null)
                return null;

            return GetTypeFromXmlArrayItemAttributeWithMatchingName(source, target)
                   ?? GetTypeFromXmlArrayItemAttributeWithoutName(target);
        }

        private static Type GetActualTypeFromXmlAttributeAttribute(XAttribute source, IMemberContext target)
        {
            if (source == null)
                return null;

            return GetTypeFromXmlAttributeAttributeWithMatchingName(source, target)
                   ?? GetTypeFromXmlAttributeAttributeWithoutName(target);
        }

        private static Type GetActualTypeFromXmlElementAttribute(XElement source, IMemberContext target)
        {
            if (source == null)
                return null;

            return GetTypeFromXmlElementAttributeWithMatchingName(source, target)
                   ?? GetTypeFromXmlElementAttributeWithoutName(target);
        }

        private static Type GetTypeFromXmlArrayItemAttributeWithMatchingName(XElement source, IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlArrayItemAttribute>()
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Where(a => a.ElementName == source.Name)
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }

        private static Type GetTypeFromXmlArrayItemAttributeWithoutName(IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlArrayItemAttribute>()
                    .Where(a => string.IsNullOrWhiteSpace(a.ElementName))
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }

        private static Type GetTypeFromXmlAttributeAttributeWithMatchingName(XAttribute source, IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlAttributeAttribute>()
                    .Where(a => !string.IsNullOrWhiteSpace(a.AttributeName))
                    .Where(a => a.AttributeName == source.Name)
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }

        private static Type GetTypeFromXmlAttributeAttributeWithoutName(IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlAttributeAttribute>()
                    .Where(a => string.IsNullOrWhiteSpace(a.AttributeName))
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }

        private static Type GetTypeFromXmlElementAttributeWithMatchingName(XElement source, IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlElementAttribute>()
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Where(a => a.ElementName == source.Name)
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }

        private static Type GetTypeFromXmlElementAttributeWithoutName(IMemberContext target)
        {
            return target.Attributes
                    .OfType<XmlElementAttribute>()
                    .Where(a => string.IsNullOrWhiteSpace(a.ElementName))
                    .Where(a => a.Type != null)
                    .Where(a => target.ContractType.IsAssignableFrom(a.Type))
                    .Select(a => a.Type)
                    .FirstOrDefault();
        }
    }
}