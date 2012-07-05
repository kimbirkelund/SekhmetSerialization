using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.TestSerializationOfBar
{
    public class TestSerializationOfBarTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedValue1 = "Foo";
        private const int ExpectedValue2 = 42;

        private readonly DateTime _expectedValue3 = new DateTime(2012, 3, 27, 10, 34, 59);
        private readonly TimeSpan _expectedValue4 = new TimeSpan(1, 12, 34, 42);

        protected override object CreateObject()
        {
            return new Bar
            {
                Value1 = ExpectedValue1,
                Value2 = ExpectedValue2,
                Value3 = _expectedValue3,
                Value4 = _expectedValue4
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Bar",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value1", ExpectedValue1),
                                new XAttribute("Id", ExpectedValue2),
                                new XElement("Value3", _expectedValue3),
                                new XElement("TimeSpan", _expectedValue4));
        }
    }
}