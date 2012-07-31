using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveDeserializerSelector : IDeserializerSelector
    {
        private readonly IDeserializer _deserializer;
        private readonly ITypeConverter _typeConverter;

        public RecursiveDeserializerSelector(ITypeConverter typeConverter, IDeserializer deserializer)
        {
            if (typeConverter == null)
                throw new ArgumentNullException("typeConverter");
            if (deserializer == null)
                throw new ArgumentNullException("deserializer");

            _typeConverter = typeConverter;
            _deserializer = deserializer;
        }

        public RecursiveDeserializerSelector(IMapper mapper, IObjectContextFactory objectContextFactory, IDeserializerSelector recursiveSelector, ITypeConverter typeConverter)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");
            if (recursiveSelector == null)
                throw new ArgumentNullException("recursiveSelector");
            if (typeConverter == null)
                throw new ArgumentNullException("typeConverter");

            _typeConverter = typeConverter;
            _deserializer = new RecursiveDeserializer(mapper, recursiveSelector, objectContextFactory, typeConverter);
        }

        public IDeserializer Select(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                return null;
            if (source.NodeType != XmlNodeType.Element)
                return null;

            var actualType = GetActualType(target, source, adviceRequester);
            if (actualType == null)
                return null;

            return _deserializer;
        }

        private Type GetActualType(IMemberContext target, XObject source, IAdviceRequester adviceRequester)
        {
            return _typeConverter.GetActualType(source, target, adviceRequester);
        }
    }
}