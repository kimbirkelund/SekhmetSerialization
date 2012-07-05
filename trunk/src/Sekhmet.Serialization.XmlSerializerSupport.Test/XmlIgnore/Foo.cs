using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlIgnore
{
    public class Foo
    {
        [XmlIgnore]
        public IManuallyParsed Value { get; set; }

        [XmlElement("Value")]
        public string ValueString
        {
            get { return Value.Value1 + "|" + Value.Value2; }
            set
            {
                string[] parts = value.Split('|');

                Value = new ManuallyParsed(int.Parse(parts[0]), parts[1]);
            }
        }
    }
}