using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonPublicMembersSerialization
{
    [XmlRoot("Foo")]
    public class Foo
    {
        [XmlArray("Bars")]
        [XmlArrayItem("SomeBar")]
        private Bar[] Bars2 { get; set; }

        [XmlIgnore]
        public Bar[] PBars2
        {
            get { return Bars2; }
            set { Bars2 = value; }
        }
    }
}