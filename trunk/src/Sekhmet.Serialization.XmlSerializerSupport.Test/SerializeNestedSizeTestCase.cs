using System.Drawing;
using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class SerializeNestedSizeTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithSize
            {
                Size = new Size(128, 256)
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithSize",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Size",
                                             new XElement("Height", 256),
                                             new XElement("Width", 128)));
        }
    }
}