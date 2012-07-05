using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.TypeFromAttributeSerializationSupport
{
    public class Bar2 : IBar
    {
        [XmlAttribute("type")]
        public string Type
        {
            get { return typeof(Bar2).AssemblyQualifiedName; }
            private set { }
        }

        public string Value2 { get; set; }

        public override string ToString()
        {
            return "Bar2" + Value2;
        }
    }
}