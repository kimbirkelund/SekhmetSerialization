using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializableDeserializerSelector : IDeserializerSelector
    {
        private readonly XmlSerializableDeserializer _deserializer;
        private readonly ITypeConverter _typeConverter;

        public XmlSerializableDeserializerSelector(ICompositeObjectContextFactory objectContextFactory, ITypeConverter typeConverter)
        {
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");
            if (typeConverter == null)
                throw new ArgumentNullException("typeConverter");

            _deserializer = new XmlSerializableDeserializer(objectContextFactory, typeConverter);
            _typeConverter = typeConverter;
        }

        public IDeserializer Select(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                return null;
            if (source.NodeType != XmlNodeType.Element)
                return null;

            var actualType = _typeConverter.GetActualType(source, target, adviceRequester);
            if (actualType == null)
                return null;

            if (!actualType.IsSubTypeOf<IXmlSerializable>())
                return null;

            return _deserializer;
        }
    }
}