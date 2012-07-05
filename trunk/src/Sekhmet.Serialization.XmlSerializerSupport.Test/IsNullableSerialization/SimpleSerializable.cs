using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.IsNullableSerialization
{
    public class SimpleSerializable : IXmlSerializable
    {
        public string Value { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Value = reader.HasAttributes ? reader.GetAttribute("id") : null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", Value);
        }
    }
}