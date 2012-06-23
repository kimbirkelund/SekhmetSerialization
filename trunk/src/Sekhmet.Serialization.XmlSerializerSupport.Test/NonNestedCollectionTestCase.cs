using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class NonNestedCollectionTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithNonNestedList
            {
                Bars = new List<SimpleBar> {
                                                                               new SimpleBar {Value = "a"},
                                                                               new SimpleBar {Value = "b"},
                                                                               new SimpleBar {Value = "c"}
                                                                           }
            };
        }

        protected override XElement CreateXml()
        {
            return XElement.Parse(@"
<FooWithNonNestedList xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Bar>
    <Value>a</Value>
  </Bar>
  <Bar>
    <Value>b</Value>
  </Bar>
  <Bar>
    <Value>c</Value>
  </Bar>
</FooWithNonNestedList>
");
        }
    }
}