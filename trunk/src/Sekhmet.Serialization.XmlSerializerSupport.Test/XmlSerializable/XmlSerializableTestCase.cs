using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializable
{
    public class XmlSerializableTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedValue1 = "Foo";
        private const int ExpectedValue2 = 24;

        protected override object CreateObject()
        {
            return new FooWithXmlSerialiableBar
            {
                Bar = new XmlSerializableBar
                {
                    ReadXmlCalled = true,
                    Value1 = ExpectedValue1,
                    Value2 = ExpectedValue2
                }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar",
                                             new XElement("Value1", ExpectedValue1),
                                             new XElement("Value2", ExpectedValue2)));
        }
    }
}