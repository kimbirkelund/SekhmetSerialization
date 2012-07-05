using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableCollectionType
{
    public class Foo
    {
        [XmlElement("List")]
        public XmlSerializableList List { get; set; }
    }
}