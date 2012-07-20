using System;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.CaseInsensitivity
{
    public class CaseInsensitivityTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedBar1Value1 = "Foo";
        private const int ExpectedBar1Value2 = 42;
        private const string ExpectedBar2Value1 = "Foo2";
        private const int ExpectedBar2Value2 = 24;

        private readonly DateTime _expectedBar1Value3 = new DateTime(2012, 3, 27, 10, 34, 59);
        private readonly TimeSpan _expectedBar1Value4 = new TimeSpan(1, 12, 34, 42);
        private readonly DateTime _expectedBar2Value3 = new DateTime(2011, 2, 26, 9, 33, 58);
        private readonly TimeSpan _expectedBar2Value4 = new TimeSpan(11, 33, 24);

        public override void AssertCorrectXml(XElement actual)
        {
            AssertCorrectXml(new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar1",
                                             new XElement("Value1", ExpectedBar1Value1),
                                             new XAttribute("Id", ExpectedBar1Value2),
                                             new XElement("Value3", _expectedBar1Value3),
                                             new XElement("TimeSpan", _expectedBar1Value4)),
                                new XElement("Bar2",
                                             new XElement("Value1", ExpectedBar2Value1),
                                             new XAttribute("Id", ExpectedBar2Value2),
                                             new XElement("Value3", _expectedBar2Value3),
                                             new XElement("TimeSpan", _expectedBar2Value4))), actual);
        }

        protected override object CreateObject()
        {
            return new Foo
            {
                Bar1 = new Bar
                {
                    Value1 = ExpectedBar1Value1,
                    Value2 = ExpectedBar1Value2,
                    Value3 = _expectedBar1Value3,
                    Value4 = _expectedBar1Value4
                },
                Bar2 = new Bar
                {
                    Value1 = ExpectedBar2Value1,
                    Value2 = ExpectedBar2Value2,
                    Value3 = _expectedBar2Value3,
                    Value4 = _expectedBar2Value4
                }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("bar1",
                                             new XElement("value1", ExpectedBar1Value1),
                                             new XAttribute("id", ExpectedBar1Value2),
                                             new XElement("value3", _expectedBar1Value3),
                                             new XElement("timespan", _expectedBar1Value4)),
                                new XElement("bar2",
                                             new XElement("value1", ExpectedBar2Value1),
                                             new XAttribute("id", ExpectedBar2Value2),
                                             new XElement("value3", _expectedBar2Value3),
                                             new XElement("timespan", _expectedBar2Value4)));
        }
    }
}