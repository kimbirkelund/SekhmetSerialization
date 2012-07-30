using System;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonPublicMembersSerialization
{
    public class Bar
    {
        [XmlAttribute("Id")]
        private int _value2;

        [XmlElement("TimeSpan")]
        private TimeSpan Value4 { get; set; }

        [XmlIgnore]
        public int PValue2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        [XmlIgnore]
        public TimeSpan PValue4
        {
            get { return Value4; }
            set { Value4 = value; }
        }
    }
}