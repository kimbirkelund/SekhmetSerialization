using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ArraySerializationSupport
{
    [XmlRoot("Foo")]
    public class Foo
    {
        public Bar[] Bars1 { get; set; }

        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        public Bar[] Bars2 { get; set; }
    }
}