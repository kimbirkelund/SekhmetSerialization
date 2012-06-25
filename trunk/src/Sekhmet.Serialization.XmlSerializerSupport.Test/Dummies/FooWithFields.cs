using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class FooWithFields
    {
        [XmlElement(ElementName = "Value1")]
        private string _value1;

        public string _value2;

        [XmlAttribute(AttributeName = "Value3")]
        private string _value3;

        [XmlIgnore]
        public string _value4;

        [XmlIgnore]
        public string Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        [XmlIgnore]
        public string Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        [XmlIgnore]
        public string Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }

        [XmlIgnore]
        public string Value4
        {
            get { return _value4; }
            set { _value4 = value; }
        }
    }
}