using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public static class Constants
    {
        public static readonly XAttribute XmlSchemaInstanceNamespaceAttribute = new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance");
        public static readonly XAttribute XmlSchemaNamespaceAttribute = new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema");
        public static readonly XAttribute XsiNilAttribute = new XAttribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"), "true");
    }
}