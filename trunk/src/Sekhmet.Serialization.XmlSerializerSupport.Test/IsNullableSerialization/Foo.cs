using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.IsNullableSerialization
{
    public class Foo
    {
        [XmlElement(IsNullable = true)]
        public SimpleSerializable Value { get; set; }
    }
}