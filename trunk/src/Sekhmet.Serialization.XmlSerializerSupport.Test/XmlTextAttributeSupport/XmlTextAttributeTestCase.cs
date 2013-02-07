using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlTextAttributeSupport
{
    public class XmlTextAttributeTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Bar
            {
                Foo1 = new Foo { Value = "value1" },
                Foo2 = new Foo { Value = "value2" }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Bar",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Foo1", "value1"),
                                new XElement("Foo2", "value2"));
        }
    }
}