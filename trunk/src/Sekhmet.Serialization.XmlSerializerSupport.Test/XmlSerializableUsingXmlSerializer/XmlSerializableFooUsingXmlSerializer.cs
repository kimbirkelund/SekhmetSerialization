using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableUsingXmlSerializer
{
    public class XmlSerializableFooUsingXmlSerializer : List<Bar>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XElement elem = XElement.Load(reader);

            var xmlSerializer = new XmlSerializer(typeof(Bar));

            foreach (XElement child in elem.Elements())
                Add((Bar)xmlSerializer.Deserialize(child.CreateReader()));
        }

        public void WriteXml(XmlWriter writer)
        {
            var xmlSerializer = new XmlSerializer(typeof(Bar));

            foreach (Bar item in this)
                xmlSerializer.Serialize(writer, item);
        }
    }
}