using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithBarBase
    {
        [XmlElement("Baz1", Type = typeof(Bar1))]
        [XmlElement("Baz2", Type = typeof(Bar2))]
        public IBar Bar { get; set; }

        [XmlArrayItem("Baz1", typeof(Bar1))]
        [XmlArrayItem("Baz2", typeof(Bar2))]
        public IEnumerable<IBar> Bars { get; set; }
    }
}