using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializable
{
    [XmlRoot("Foo")]
    public class FooWithXmlSerialiableBar
    {
        public XmlSerializableBar Bar { get; set; }
    }
}