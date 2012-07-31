using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlParent
{
    public class Bar
    {
        [XmlParent]
        private Foo _parent;

        [XmlIgnore]
        public Foo PParent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        [XmlParent]
        public Foo Parent { get; set; }
    }
}