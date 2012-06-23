using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithArrays
    {
        public Bar[] Bars1 { get; set; }

        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        public Bar[] Bars2 { get; set; }
    }
}