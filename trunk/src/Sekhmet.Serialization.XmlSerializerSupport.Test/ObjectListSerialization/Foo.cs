using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ObjectListSerialization
{
    public class Foo
    {
        [XmlElement("string", typeof(string))]
        [XmlElement("Bar", typeof(Bar))]
        public object[] Values { get; set; }
    }
}