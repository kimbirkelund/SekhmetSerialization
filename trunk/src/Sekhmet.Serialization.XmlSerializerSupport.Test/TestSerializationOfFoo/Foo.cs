using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.TestSerializationOfFoo
{
    [XmlRoot("Foo")]
    public class Foo
    {
        public Bar Bar1 { get; set; }

        public Bar Bar2 { get; set; }
    }
}