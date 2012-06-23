using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithXmlSerialiableBar
    {
        public XmlSerializableBar Bar { get; set; }
    }
}