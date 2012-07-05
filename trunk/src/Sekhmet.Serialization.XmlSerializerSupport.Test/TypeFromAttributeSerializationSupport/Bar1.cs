using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.TypeFromAttributeSerializationSupport
{
    public class Bar1 : IBar
    {
        [XmlAttribute("type")]
        public string Type
        {
            get { return typeof(Bar1).AssemblyQualifiedName; }
            private set { }
        }

        public string Value1 { get; set; }

        public override string ToString()
        {
            return "Bar1" + Value1;
        }
    }
}