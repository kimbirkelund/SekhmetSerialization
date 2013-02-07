using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlTextAttributeSupport
{
    public class Foo
    {
        [XmlText]
        public string Value { get; set; }
    }
}