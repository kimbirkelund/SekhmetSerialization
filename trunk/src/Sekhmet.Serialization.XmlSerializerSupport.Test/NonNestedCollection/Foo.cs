using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonNestedCollection
{
    public class Foo
    {
        [XmlElement("Bar")]
        public List<Bar> Bars { get; set; }
    }
}