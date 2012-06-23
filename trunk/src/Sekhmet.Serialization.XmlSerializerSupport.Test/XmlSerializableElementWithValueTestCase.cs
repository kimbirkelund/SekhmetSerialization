using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class XmlSerializableElementWithValueTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new XmlSerializableFooWithElementValueAndAttribute
            {
                Id = "42",
                Value = "foo"
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("XmlSerializableFooWithElementValueAndAttribute",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XAttribute("id", 42),
                                "foo");
        }
    }

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