using System;
using System.Xml.Linq;
using Common.Logging;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CollectionSerializerSelector : ISerializerSelector
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

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

            if (_log.IsDebugEnabled)
                _log.Debug("Selected collection serialization for source '" + source + "' and target '" + target.ToFriendlyName() + "'.");

            return _serializer;
        }
    }
}