namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlIgnore
{
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