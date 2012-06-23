using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;
using Xunit;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    public class NonNestedCollectionAndAnotherPropertyTest
    {
        private readonly XElement _expected = XElement.Parse(@"
<FooWithNonNestedListAndProperty xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Bar>
    <Value>a</Value>
  </Bar>
  <Bar>
    <Value>b</Value>
  </Bar>
  <Bar>
    <Value>c</Value>
  </Bar>
  <Bar2>
    <Value>d</Value>
  </Bar2>
</FooWithNonNestedListAndProperty>
");

        [Fact]
        public void TestDeserialize()
        {
            var xml = _expected;

            var xmlSerializer = new XmlSerializer(typeof(Dummies.FooWithNonNestedListAndProperty));

            var foo = (Dummies.FooWithNonNestedListAndProperty)xmlSerializer.Deserialize(xml.CreateReader());

            Assert.Equal(3, foo.Bars.Count);
            Assert.Equal("a", foo.Bars[0].Value);
            Assert.Equal("b", foo.Bars[1].Value);
            Assert.Equal("c", foo.Bars[2].Value);
            Assert.Equal("d", foo.Bar.Value);
        }

        [Fact]
        public void TestSerialize()
        {
            var foo = new Dummies.FooWithNonNestedListAndProperty
            {
                Bars = new List<SimpleBar> {
                                                                                                                 new SimpleBar {Value = "a"},
                                                                                                                 new SimpleBar {Value = "b"},
                                                                                                                 new SimpleBar {Value = "c"}
                                                                                                             },
                Bar = new SimpleBar { Value = "d" }
            };

            var xmlSerializer = new XmlSerializer(typeof(Dummies.FooWithNonNestedListAndProperty));

            var stream = new MemoryStream();

            xmlSerializer.Serialize(stream, foo);

            stream.Position = 0;
            var actual = XElement.Load(stream);
            Console.WriteLine(actual);

            Assert.True(XNode.DeepEquals(_expected, actual));
        }
    }
}