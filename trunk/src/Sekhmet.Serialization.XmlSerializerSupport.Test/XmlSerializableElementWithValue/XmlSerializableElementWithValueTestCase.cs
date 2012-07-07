using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableElementWithValue
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
}