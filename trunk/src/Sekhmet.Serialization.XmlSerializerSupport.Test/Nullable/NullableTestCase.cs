using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Nullable
{
    public class NullableTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Value1 = null,
                Value2 = 42
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value1", Constants.XsiNilAttribute),
                                new XElement("Value2", 42));
        }
    }
}