using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class ObjectListSerializationTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithObjectArray
            {
                Values = new object[]{
                                                                              "Bob",
                                                                              new SimpleBar{Value = "hello2"},
                                                                              "Foo",
                                                                              new SimpleBar{Value = "hello"},
                                                                      }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithObjectArray",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("string", "Bob"),
                                new XElement("Bar",
                                             new XElement("Value", "hello2")),
                                new XElement("string", "Foo"),
                                new XElement("Bar",
                                             new XElement("Value", "hello")));
        }
    }

    public class FooWithObjectArray
    {
        [XmlElement("string", typeof(string))]
        [XmlElement("Bar", typeof(SimpleBar))]
        public object[] Values { get; set; }
    }
}