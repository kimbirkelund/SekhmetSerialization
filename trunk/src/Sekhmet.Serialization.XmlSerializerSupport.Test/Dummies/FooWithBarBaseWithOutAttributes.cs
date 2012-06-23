using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    [XmlRoot("Foo")]
    public class FooWithBarBaseWithOutAttributes
    {
        public IBar Bar { get; set; }

        public IEnumerable<IBar> Bars { get; set; }
    }
}