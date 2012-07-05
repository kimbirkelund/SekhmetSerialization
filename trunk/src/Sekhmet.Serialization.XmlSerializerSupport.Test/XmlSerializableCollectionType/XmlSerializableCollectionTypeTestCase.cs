using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableCollectionType
{
    public class XmlSerializableCollectionTypeTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                List = new XmlSerializableList {
                                                                                         new Bar {
                                                                                                     Value1 = "a",
                                                                                                     Value2 = 42,
                                                                                                     Value3 = new DateTime(2012, 12, 31),
                                                                                                     Value4 = TimeSpan.FromMilliseconds(2342348230)
                                                                                                 },
                                                                                         new Bar {
                                                                                                     Value1 = "b",
                                                                                                     Value2 = 24,
                                                                                                     Value3 = new DateTime(2011, 11, 30),
                                                                                                     Value4 = TimeSpan.FromMilliseconds(4823023423)
                                                                                                 },
                                                                                     }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("List",
                                             new XElement("Bar",
                                                          new XElement("Value1", "a"),
                                                          new XAttribute("Id", 42),
                                                          new XElement("Value3", new DateTime(2012, 12, 31)),
                                                          new XElement("TimeSpan", TimeSpan.FromMilliseconds(2342348230))),
                                             new XElement("Bar",
                                                          new XElement("Value1", "b"),
                                                          new XAttribute("Id", 24),
                                                          new XElement("Value3", new DateTime(2011, 11, 30)),
                                                          new XElement("TimeSpan", TimeSpan.FromMilliseconds(4823023423)))));
        }
    }
}