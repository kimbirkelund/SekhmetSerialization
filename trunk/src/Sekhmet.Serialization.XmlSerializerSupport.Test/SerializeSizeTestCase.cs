using System.Drawing;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class SerializeSizeTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Size(128, 256);
        }

        protected override XElement CreateXml()
        {
            return new XElement("Size",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Height", 256),
                                new XElement("Width", 128));
        }
    }
}