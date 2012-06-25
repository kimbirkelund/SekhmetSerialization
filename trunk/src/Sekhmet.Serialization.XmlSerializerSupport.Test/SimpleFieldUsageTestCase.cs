using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class SimpleFieldUsageTestCase : XmlSerializerSerializationTestCaseBase
    {
        public override void AssertCorrectObject(object actual)
        {
            AssertCorrectObject(new FooWithFields
            {
                Value1 = "foo1",
                Value3 = "foo3",
            }, actual);
        }

        public override void AssertCorrectXml(XElement actual)
        {
            AssertCorrectXml(new XElement("FooWithFields",
                                          Constants.XmlSchemaInstanceNamespaceAttribute,
                                          new XElement("Value1", "foo1"),
                                          new XAttribute("Value3", "foo3")), actual);
        }

        protected override object CreateObject()
        {
            return new FooWithFields
            {
                Value1 = "foo1",
                Value2 = "foo2",
                Value3 = "foo3",
                Value4 = "foo4",
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithFields",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value1", "foo1"),
                                new XElement("_value2", "foo2"),
                                new XAttribute("Value3", "foo3"),
                                new XAttribute("_value4", "foo4"));
        }
    }
}