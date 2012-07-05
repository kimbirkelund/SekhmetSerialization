using System.Drawing;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.SerializeNestedSize
{
    public class SerializeNestedSizeTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Foo
            {
                Size = new Size(128, 256)
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Size",
                                             new XElement("Height", 256),
                                             new XElement("Width", 128)));
        }
    }
}