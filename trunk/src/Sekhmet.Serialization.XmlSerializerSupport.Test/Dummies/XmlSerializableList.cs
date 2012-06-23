using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies
{
    public class XmlSerializableList : List<Bar>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elem = XElement.Load(reader);
            var manager = new XmlSerializerSerializationManagerFactory().Create();

            foreach (var child in elem.Elements())
                Add(manager.Deserialize<Bar>(child));
        }

        public void WriteXml(XmlWriter writer)
        {
            var manager = new XmlSerializerSerializationManagerFactory().Create();

            foreach (var child in this)
            {
                var elem = manager.Serialize(child);

                elem.WriteTo(writer);
            }
        }
    }
}