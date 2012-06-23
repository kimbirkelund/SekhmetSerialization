using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithNonNestedList
    {
        [XmlElement("Bar")]
        public List<SimpleBar> Bars { get; set; }
    }
}