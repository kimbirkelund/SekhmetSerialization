using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class NonNestedCollectionAndAnotherPropertyTest
    {
        private readonly XElement _expected = XElement.Parse(@"
<FooWithNonNestedListAndProperty>
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

        [Test]
        public void TestDeserialize()
        {
            var xml = _expected;

            var xmlSerializer = new XmlSerializer(typeof(Dummies.FooWithNonNestedListAndProperty));

            var foo = (Dummies.FooWithNonNestedListAndProperty)xmlSerializer.Deserialize(xml.CreateReader());

            Assert.AreEqual(3, foo.Bars.Count);
            Assert.AreEqual("a", foo.Bars[0].Value);
            Assert.AreEqual("b", foo.Bars[1].Value);
            Assert.AreEqual("c", foo.Bars[2].Value);
            Assert.AreEqual("d", foo.Bar.Value);
        }

        [Test]
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

            XAttribute attrXsi = actual.Attribute(XName.Get("xsi", "http://www.w3.org/2000/xmlns/"));
            Assert.NotNull(attrXsi);
            Assert.AreEqual("http://www.w3.org/2001/XMLSchema-instance", attrXsi.Value);
            attrXsi.Remove();

            XAttribute attrXsd = actual.Attribute(XName.Get("xsd", "http://www.w3.org/2000/xmlns/"));
            Assert.NotNull(attrXsd);
            Assert.AreEqual("http://www.w3.org/2001/XMLSchema", attrXsd.Value);
            attrXsd.Remove();
            
            Console.WriteLine("Expected: [" + _expected + "]");
            Console.WriteLine("Actual: [" + actual + "]");

            Assert.True(XNode.DeepEquals(_expected, actual));
        }
    }
}