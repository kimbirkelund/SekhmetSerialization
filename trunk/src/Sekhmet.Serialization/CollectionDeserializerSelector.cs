using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class CollectionDeserializerSelector : IDeserializerSelector
    {
        private readonly RecursiveDeserializer _deserializer;
        private readonly BuiltInCollectionsTypeConverter _typeConverter = new BuiltInCollectionsTypeConverter();

        public CollectionDeserializerSelector(IMapper mapper, IDeserializerSelector recursiveSelector, IObjectContextFactory objectContextFactory)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (recursiveSelector == null)
                throw new ArgumentNullException("recursiveSelector");
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");

            _deserializer = new RecursiveDeserializer(mapper, recursiveSelector, objectContextFactory, _typeConverter);
        }

        public IDeserializer Select(XObject source, IMemberContext target)
        {
            var actualType = _typeConverter.GetActualType(source, target);
            if (actualType == null)
                return null;

            return _deserializer;
        }
    }
}