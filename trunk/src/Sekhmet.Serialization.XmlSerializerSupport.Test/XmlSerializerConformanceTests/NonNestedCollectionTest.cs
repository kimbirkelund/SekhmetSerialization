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
    public class NonNestedCollectionTest
    {
        private readonly XElement _expected = XElement.Parse(@"
<FooWithNonNestedList xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
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

        [Test]
        public void TestDeserialize()
        {
            var xml = _expected;

            var xmlSerializer = new XmlSerializer(typeof (FooWithNonNestedList));

            var foo = (FooWithNonNestedList) xmlSerializer.Deserialize(xml.CreateReader());

            Assert.AreEqual(3, foo.Bars.Count);
            Assert.AreEqual("a", foo.Bars[0].Value);
            Assert.AreEqual("b", foo.Bars[1].Value);
            Assert.AreEqual("c", foo.Bars[2].Value);
        }

        [Test]
        public void TestSerialize()
        {
            var foo = new FooWithNonNestedList {
                                                   Bars = new List<SimpleBar> {
                                                                                  new SimpleBar {Value = "a"},
                                                                                  new SimpleBar {Value = "b"},
                                                                                  new SimpleBar {Value = "c"}
                                                                              }
                                               };

            var xmlSerializer = new XmlSerializer(typeof (FooWithNonNestedList));

            var stream = new MemoryStream();

            xmlSerializer.Serialize(stream, foo);

            stream.Position = 0;
            var actual = XElement.Load(stream);
            Console.WriteLine(actual);

            Assert.True(XNode.DeepEquals(_expected, actual));
        }
    }
}