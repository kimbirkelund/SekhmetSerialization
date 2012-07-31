using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Sekhmet.Serialization.Advicing;
using Sekhmet.Serialization.Utility;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    [TestFixture]
    public class MultipleMatchingElementsTest
    {
        [Test]
        public void TestExceptionThrownOnMultipleMatchingElements()
        {
            const string bar11Value1 = "Foo1";
            const int bar11Value2 = 42;
            var bar11Value3 = new DateTime(2012, 3, 27, 10, 34, 59);
            var bar11Value4 = new TimeSpan(1, 12, 34, 42);

            const string bar12Value1 = "Foo2";
            const int bar12Value2 = 24;
            var bar12Value3 = new DateTime(2012, 3, 27, 10, 43, 59);
            var bar12Value4 = new TimeSpan(1, 12, 34, 24);

            const string bar2Value1 = "Foo3";
            const int bar2Value2 = 25;
            var bar2Value3 = new DateTime(2011, 2, 26, 9, 33, 58);
            var bar2Value4 = new TimeSpan(11, 33, 24);

            var elem = new XElement("Foo",
                                    Constants.XmlSchemaInstanceNamespaceAttribute,
                                    new XElement("Bar1",
                                                 new XElement("Value1", bar11Value1),
                                                 new XAttribute("Id", bar11Value2),
                                                 new XElement("Value3", bar11Value3),
                                                 new XElement("TimeSpan", bar11Value4)),
                                    new XElement("Bar1",
                                                 new XElement("Value1", bar12Value1),
                                                 new XAttribute("Id", bar12Value2),
                                                 new XElement("Value3", bar12Value3),
                                                 new XElement("TimeSpan", bar12Value4)),
                                    new XElement("Bar2",
                                                 new XElement("Value1", bar2Value1),
                                                 new XAttribute("Id", bar2Value2),
                                                 new XElement("Value3", bar2Value3),
                                                 new XElement("TimeSpan", bar2Value4)));

            ISerializationManager manager = new XmlSerializerSerializationManagerFactory().Create();

            manager.AddAdvisor((s, e) =>
            {
                var args = (MultipleMatchesAdviceRequestedEventArgs)e;
            
                Assert.AreEqual(2, args.Matches.Count());
                var elemBar1 = (XElement)args.Matches.First();
                XElement elemBar1Value1 = elemBar1.Element("Value1");
                Assert.NotNull(elemBar1Value1);
                Assert.AreEqual(bar11Value1, elemBar1Value1.Value);

                args.SelectedMatch = args.Matches.Last();
            }, CommonAdviceTypes.MultipleMatches);

            var result = manager.Deserialize<Foo>(elem);

            Assert.NotNull(result);
            Assert.NotNull(result.Bar1);
            Assert.AreEqual(bar12Value1, result.Bar1.Value1);
            Assert.AreEqual(bar12Value2, result.Bar1.Value2);
            Assert.AreEqual(bar12Value3, result.Bar1.Value3);
            Assert.AreEqual(bar12Value4, result.Bar1.Value4);

            Assert.NotNull(result.Bar2);
            Assert.AreEqual(bar2Value1, result.Bar2.Value1);
            Assert.AreEqual(bar2Value2, result.Bar2.Value2);
            Assert.AreEqual(bar2Value3, result.Bar2.Value3);
            Assert.AreEqual(bar2Value4, result.Bar2.Value4);
        }
    }
}