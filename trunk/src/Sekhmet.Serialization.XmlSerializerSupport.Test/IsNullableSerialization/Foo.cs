using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.IsNullableSerialization
{
    public class Foo
    {
        [XmlElement(IsNullable = false)]
        public int? NotNullableId { get; set; }

        [XmlElement(IsNullable = true)]
        public int? NullableId { get; set; }

        [XmlElement(IsNullable = false)]
        public int? NotNullableIdWithValue { get; set; }

        [XmlElement(IsNullable = true)]
        public int? NullableIdWithValue { get; set; }

        [XmlElement(IsNullable = true)]
        public SimpleSerializable Value { get; set; }
    }
}