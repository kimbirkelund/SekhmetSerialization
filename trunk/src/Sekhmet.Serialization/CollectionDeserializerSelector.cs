using System;
using System.Xml.Linq;
using Common.Logging;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CollectionDeserializerSelector : IDeserializerSelector
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

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

        public IDeserializer Select(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            Type actualType = _typeConverter.GetActualType(source, target, adviceRequester);
            if (actualType == null)
                return null;

            if (_log.IsDebugEnabled)
                _log.Debug("Selected collection deserialization for source '" + source.ToFriendlyName() + "' and target '" + target + "'.");

            return _deserializer;
        }
    }
}