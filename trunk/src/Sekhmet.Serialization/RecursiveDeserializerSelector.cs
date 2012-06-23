using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveDeserializerSelector : IDeserializerSelector
    {
        private readonly RecursiveDeserializer _deserializer;
        private readonly ITypeConverter _typeConverter;

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

        public IDeserializer Select(XObject source, IMemberContext target)
        {
            if (source == null)
                return null;
            if (source.NodeType != XmlNodeType.Element)
                return null;

            var actualType = GetActualType(target, source);
            if (actualType == null)
                return null;

            return _deserializer;
        }

        private Type GetActualType(IMemberContext target, XObject source)
        {
            return _typeConverter.GetActualType(source, target);
        }
    }
}