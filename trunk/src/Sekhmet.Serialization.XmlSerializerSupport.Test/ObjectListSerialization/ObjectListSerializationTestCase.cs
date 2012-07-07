using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ObjectListSerialization
{
    public class ObjectListSerializationTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Values = new object[] {
                                                                        "Bob",
                                                                        new Bar {Value = "hello2"},
                                                                        "Foo",
                                                                        new Bar {Value = "hello"}
                                                                    }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("string", "Bob"),
                                new XElement("Bar",
                                             new XElement("Value", "hello2")),
                                new XElement("string", "Foo"),
                                new XElement("Bar",
                                             new XElement("Value", "hello")));
        }
    }
}