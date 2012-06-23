using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithReadOnlyProperty
    {
        [XmlElement]
        public string Value1
        {
            get { return "Bob"; }
        }

        [XmlAttribute]
        public string Value2
        {
            get { return "Bob"; }
        }
    }
}