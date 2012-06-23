using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithNonIListLists
    {
        public NonIListList<Bar> Bars1 { get; set; }

        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        public NonIListList<Bar> Bars2 { get; set; }
    }
}