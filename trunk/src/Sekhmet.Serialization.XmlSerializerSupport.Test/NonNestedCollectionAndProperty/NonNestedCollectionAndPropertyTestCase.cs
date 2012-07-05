using System.Collections.Generic;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonNestedCollectionAndProperty
{
    public class NonNestedCollectionAndPropertyTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Bars = new List<Bar> {
                                                        new Bar {Value = "a"},
                                                        new Bar {Value = "b"},
                                                        new Bar {Value = "c"}
                                                    },
                Bar = new Bar { Value = "d" }
            };
        }

        protected override XElement CreateXml()
        {
            return XElement.Parse(@"
<Foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
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
</Foo>
");
        }
    }
}