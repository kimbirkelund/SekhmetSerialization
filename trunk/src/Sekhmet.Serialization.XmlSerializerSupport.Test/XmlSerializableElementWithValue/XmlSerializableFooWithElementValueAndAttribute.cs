using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableElementWithValue
{
    public class XmlSerializableFooWithElementValueAndAttribute : IXmlSerializable
    {
        public string Id { get; set; }
        public string Value { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Id = reader.HasAttributes ? reader.GetAttribute("id") : null;
            if (reader.IsEmptyElement)
                return;

            reader.ReadStartElement();
            Value = reader.ReadString();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", Id);
            writer.WriteString(Value);
        }
    }
}