using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;
using Sekhmet.Serialization.XmlSerializerSupport.Test;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class NonNestedCollectionAndPropertyTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithNonNestedListAndProperty
            {
                Bars = new List<SimpleBar> {
                                                                                          new SimpleBar {Value = "a"},
                                                                                          new SimpleBar {Value = "b"},
                                                                                          new SimpleBar {Value = "c"}
                                                                                      },
                Bar = new SimpleBar { Value = "d" }
            };
        }

        protected override XElement CreateXml()
        {
            return XElement.Parse(@"
<FooWithNonNestedListAndProperty xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Bar2>
    <Value>d</Value>
  </Bar2>
  <Bar>
    <Value>a</Value>
  </Bar>
  <Bar>
    <Value>b</Value>
  </Bar>
  <Bar>
    <Value>c</Value>
  </Bar>
</FooWithNonNestedListAndProperty>
");
        }
    }

    public class FooWithNonNestedListAndProperty
    {
        [XmlElement("Bar2")]
        public SimpleBar Bar { get; set; }

        [XmlElement("Bar")]
        public List<SimpleBar> Bars { get; set; }
    }
}