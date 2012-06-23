using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class XmlSerializableBar : IBar, IXmlSerializable
    {
        public bool ReadXmlCalled { get; set; }
        public string Value1 { get; set; }
        public int Value2 { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadToDescendant("Value1");
            Value1 = reader.ReadString();
            reader.ReadEndElement();
            reader.ReadStartElement("Value2");
            Value2 = int.Parse(reader.ReadString());
            ReadXmlCalled = true;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Value1", Value1);
            writer.WriteElementString("Value2", Value2 + "");
        }
    }
}