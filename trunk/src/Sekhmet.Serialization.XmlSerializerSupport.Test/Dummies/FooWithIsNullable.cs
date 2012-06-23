using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithIsNullable
    {
        [XmlElement(IsNullable = true)]
        public SimpleSerializable Value { get; set; }
    }
}