using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializableDeserializer : IDeserializer
    {
        private readonly IObjectContextFactory _objectContextFactory;
        private readonly ITypeConverter _typeConverter;

        public XmlSerializableDeserializer(IObjectContextFactory objectContextFactory, ITypeConverter typeConverter)
        {
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");
            if (typeConverter == null)
                throw new ArgumentNullException("typeConverter");

            _objectContextFactory = objectContextFactory;
            _typeConverter = typeConverter;
        }

        public void Deserialize(XObject source, IMemberContext target)
        {
            var targetType = _typeConverter.GetActualType(source, target);

            var targetObject = _objectContextFactory.CreateForDeserialization(target, targetType);
            if (targetObject == null)
                throw new ArgumentException("Unable to create target object for source '" + source + "' and target '" + target + "'.");

            if (source != null)
            {
                if (source.NodeType != XmlNodeType.Element)
                    throw new ArgumentException("Parameter must be an XML element.", "source");

                var elem = (XElement)source;

                var xmlReader = elem.CreateReader();

                var xmlSerializable = (IXmlSerializable)targetObject.GetObject();

                xmlReader.Read();
                xmlSerializable.ReadXml(xmlReader);
            }

            target.SetValue(targetObject);

            target.CommitChanges();
        }
    }
}