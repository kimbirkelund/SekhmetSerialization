using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializableSerializer : ISerializer
    {
        public bool Serialize(IMemberContext source, XObject target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var sourceObject = source.GetValue().GetObject();

            if (target.NodeType != XmlNodeType.Element)
                throw new ArgumentException("Parameter must be an XML element.", "source");

            var elem = (XElement)target;

            var xmlSerializable = (IXmlSerializable)sourceObject;

            var memoryStream = new MemoryStream();

            var xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xmlWriter.WriteStartElement(elem.Name.LocalName);
            xmlWriter.WriteAttributeString("xmlns", "xsi", null, Constants.XmlSchemaInstanceNamespaceAttribute.Value);
            xmlWriter.WriteAttributeString("xmlns", "xsd", null, Constants.XmlSchemaNamespaceAttribute.Value);
            xmlSerializable.WriteXml(xmlWriter);
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            memoryStream.Position = 0;

            var tempTarget = XElement.Load(memoryStream);

            if (tempTarget.HasElements)
                foreach (var element in tempTarget.Elements())
                    elem.Add(element);
            else
                elem.SetValue(tempTarget.Value);

            foreach (var attribute in tempTarget.Attributes())
            {
                if (attribute.Name == Constants.XmlSchemaNamespaceAttribute.Name || attribute.Name == Constants.XmlSchemaInstanceNamespaceAttribute.Name)
                    continue;
                elem.Add(attribute);
            }

            foreach (var childElem in elem.Elements())
            {
                var xmlnsXsiAttr = childElem.Attribute(Constants.XmlSchemaInstanceNamespaceAttribute.Name);
                if (xmlnsXsiAttr != null)
                    xmlnsXsiAttr.Remove();

                var xmlnsXsdAttr = childElem.Attribute(Constants.XmlSchemaNamespaceAttribute.Name);
                if (xmlnsXsdAttr != null)
                    xmlnsXsdAttr.Remove();
            }

            return true;
        }
    }
}