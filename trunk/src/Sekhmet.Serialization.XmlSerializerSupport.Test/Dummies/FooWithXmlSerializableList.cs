using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithXmlSerializableList
    {
        [XmlElement("List")]
        public XmlSerializableList List { get; set; }
    }
}