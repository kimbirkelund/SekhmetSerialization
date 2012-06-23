using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithLists
    {
        public List<Bar> Bars1 { get; set; }

        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        public IEnumerable<Bar> Bars2 { get; set; }
    }
}