using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class XmlIgnoreTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new FooWithWrapperProperty
            {
                Value = new ManuallyParsed(42, "foo")
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithWrapperProperty",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value", "42|foo"));
        }
    }

    public class FooWithWrapperProperty
    {
        [XmlIgnore]
        public IManuallyParsed Value { get; set; }

        [XmlElement("Value")]
        public string ValueString
        {
            get { return Value.Value1 + "|" + Value.Value2; }
            set
            {
                var parts = value.Split('|');

                Value = new ManuallyParsed(int.Parse(parts[0]), parts[1]);
            }
        }
    }

    public interface IManuallyParsed
    {
        int Value1 { get; }
        string Value2 { get; }
    }

    public class ManuallyParsed : IManuallyParsed
    {
        public int Value1 { get; private set; }

        public string Value2 { get; private set; }

        public ManuallyParsed(int value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}