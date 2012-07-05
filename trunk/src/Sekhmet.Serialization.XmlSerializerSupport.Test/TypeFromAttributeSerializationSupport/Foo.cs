using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.TypeFromAttributeSerializationSupport
{
    [XmlRoot("Foo")]
    public class Foo
    {
        public IBar Bar { get; set; }

        public IEnumerable<IBar> Bars { get; set; }
    }
}