using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithNonNestedListAndProperty
    {
        [XmlElement("Bar")]
        public List<SimpleBar> Bars { get; set; }

        [XmlElement("Bar2")]
        public SimpleBar Bar { get; set; }
    }
}