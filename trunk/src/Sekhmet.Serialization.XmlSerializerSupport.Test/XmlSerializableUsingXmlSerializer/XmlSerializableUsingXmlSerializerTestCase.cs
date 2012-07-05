using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableUsingXmlSerializer
{
    public class XmlSerializableUsingXmlSerializerTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new XmlSerializableFooUsingXmlSerializer {
                                                                new Bar {Value = "a"},
                                                                new Bar {Value = "b"},
                                                                new Bar {Value = "c"},
                                                                new Bar {Value = "d"}
                                                            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("XmlSerializableFooUsingXmlSerializer",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar",
                                             new XElement("Value", "a")),
                                new XElement("Bar",
                                             new XElement("Value", "b")),
                                new XElement("Bar",
                                             new XElement("Value", "c")),
                                new XElement("Bar",
                                             new XElement("Value", "d")));
        }
    }
}