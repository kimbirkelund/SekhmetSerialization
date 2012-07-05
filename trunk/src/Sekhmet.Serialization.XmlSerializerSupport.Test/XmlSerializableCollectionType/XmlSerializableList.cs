using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializableCollectionType
{
    public class XmlSerializableList : List<Bar>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XElement elem = XElement.Load(reader);
            ISerializationManager manager = new XmlSerializerSerializationManagerFactory().Create();

            foreach (XElement child in elem.Elements())
                Add(manager.Deserialize<Bar>(child));
        }

        public void WriteXml(XmlWriter writer)
        {
            ISerializationManager manager = new XmlSerializerSerializationManagerFactory().Create();

            foreach (Bar child in this)
            {
                XElement elem = manager.Serialize(child);

                elem.WriteTo(writer);
            }
        }
    }
}