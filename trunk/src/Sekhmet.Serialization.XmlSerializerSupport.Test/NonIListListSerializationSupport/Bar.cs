using System;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonIListListSerializationSupport
{
    public class Bar
    {
        private static DateTime? _now;

        [XmlIgnore]
        public DateTime Now
        {
            get { return _now ?? (_now = DateTime.Now).Value; }
        }

        public string Value1 { get; set; }

        [XmlAttribute("Id")]
        public int Value2 { get; set; }

        public DateTime Value3 { get; set; }

        [XmlElement("TimeSpan")]
        public TimeSpan Value4 { get; set; }
    }
}