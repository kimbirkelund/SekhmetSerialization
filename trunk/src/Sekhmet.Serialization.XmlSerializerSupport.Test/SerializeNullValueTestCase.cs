using System;
using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class SerializeNullValueTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Bar1 = new Bar()
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar1",
                                    new XAttribute("Id", 0),
                                    new XElement("Value3", DateTime.MinValue),
                                    new XElement("TimeSpan", TimeSpan.Zero)));
        }
    }
}