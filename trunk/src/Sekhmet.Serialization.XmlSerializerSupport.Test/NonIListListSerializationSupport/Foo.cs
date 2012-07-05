using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonIListListSerializationSupport
{
    [XmlRoot("Foo")]
    public class Foo
    {
        public NonIListList<Bar> Bars1 { get; set; }

        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        public NonIListList<Bar> Bars2 { get; set; }
    }
}