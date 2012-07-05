using System.Collections.Generic;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonNestedCollection
{
    public class NonNestedCollectionTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Bars = new List<Bar> {
                                                                               new Bar {Value = "a"},
                                                                               new Bar {Value = "b"},
                                                                               new Bar {Value = "c"}
                                                                           }
            };
        }

        protected override XElement CreateXml()
        {
            return XElement.Parse(@"
<Foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Bar>
    <Value>a</Value>
  </Bar>
  <Bar>
    <Value>b</Value>
  </Bar>
  <Bar>
    <Value>c</Value>
  </Bar>
</Foo>
");
        }
    }
}