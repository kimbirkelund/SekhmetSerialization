using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class XmlSerializableUsingXmlSerializerTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new XmlSerializableFooUsingXmlSerializer {
                                                                new SimpleBar {Value = "a"},
                                                                new SimpleBar {Value = "b"},
                                                                new SimpleBar {Value = "c"},
                                                                new SimpleBar {Value = "d"}
                                                            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("XmlSerializableFooUsingXmlSerializer",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("SimpleBar",
                                             new XElement("Value", "a")),
                                new XElement("SimpleBar",
                                             new XElement("Value", "b")),
                                new XElement("SimpleBar",
                                             new XElement("Value", "c")),
                                new XElement("SimpleBar",
                                             new XElement("Value", "d")));
        }
    }

    public class XmlSerializableFooUsingXmlSerializer : List<SimpleBar>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elem = XElement.Load(reader);

            var xmlSerializer = new XmlSerializer(typeof(SimpleBar));

            foreach (var child in elem.Elements())
                Add((SimpleBar)xmlSerializer.Deserialize(child.CreateReader()));
        }

        public void WriteXml(XmlWriter writer)
        {
            var xmlSerializer = new XmlSerializer(typeof(SimpleBar));

            foreach (var item in this)
                xmlSerializer.Serialize(writer, item);
        }
    }
}