using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class NullableTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithNullable
            {
                Value1 = null,
                Value2 = 42
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithNullable",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value1", Constants.XsiNilAttribute),
                                new XElement("Value2", 42));
        }
    }

    public class FooWithNullable
    {
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
    }
}