using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class CollectionSerializerSelector : ISerializerSelector
    {
        private readonly ISerializer _serializer;
        private readonly BuiltInCollectionsTypeConverter _typeConverter = new BuiltInCollectionsTypeConverter();

        public CollectionSerializerSelector(IMapper mapper, ISerializerSelector recursiveSelector, IIsNullableStrategy isNullableStrategy = null)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (recursiveSelector == null)
                throw new ArgumentNullException("recursiveSelector");

            _serializer = new RecursiveSerializer(mapper, recursiveSelector, isNullableStrategy);
        }

        public ISerializer Select(IMemberContext source, XObject target)
        {
            var actualType = _typeConverter.GetActualType(source.GetActualType());
            if (actualType == null)
                return null;

            return _serializer;
        }
    }
}