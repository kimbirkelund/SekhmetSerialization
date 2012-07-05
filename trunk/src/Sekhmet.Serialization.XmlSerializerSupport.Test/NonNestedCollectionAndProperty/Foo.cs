using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonNestedCollectionAndProperty
{
    public class Foo
    {
        [XmlElement("Bar2")]
        public Bar Bar { get; set; }

        [XmlElement("Bar")]
        public List<Bar> Bars { get; set; }
    }
}