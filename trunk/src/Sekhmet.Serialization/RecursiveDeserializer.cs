using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Common.Logging;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class RecursiveDeserializer : IDeserializer
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;
        private readonly IObjectContextFactory _objectContextFactory;
        private readonly IDeserializerSelector _recursiveSelector;
        private readonly ITypeConverter _typeConverter;

        public RecursiveDeserializer(IMapper mapper, IDeserializerSelector recursiveSelector, IObjectContextFactory objectContextFactory, ITypeConverter typeConverter)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (recursiveSelector == null)
                throw new ArgumentNullException("recursiveSelector");
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");
            if (typeConverter == null)
                throw new ArgumentNullException("typeConverter");

            _mapper = mapper;
            _recursiveSelector = recursiveSelector;
            _objectContextFactory = objectContextFactory;
            _typeConverter = typeConverter;
        }

        public virtual void Deserialize(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                return;

            if (source.NodeType != XmlNodeType.Element)
                throw new ArgumentException("Parameter must be an XML element.", "source");

            if (_log.IsDebugEnabled)
                _log.Debug("Deserializing " + source.ToFriendlyName() + " into " + target + ".");

            var elem = (XElement)source;

            Type targetType = _typeConverter.GetActualType(source, target, adviceRequester);
            if (targetType == null)
                throw new ArgumentException("Unable to get target type for target '" + target + "'.");

            IObjectContext targetObject = _objectContextFactory.CreateForDeserialization(target, targetType, adviceRequester);
            if (targetObject == null)
                throw new ArgumentException("Unable to create target object for target '" + target + "'.");

            target.SetValue(targetObject);

            List<IMapping<XObject, IMemberContext>> mappings = _mapper.MapForDeserialization(elem, target, adviceRequester)
                .ToList();
            if (mappings == null)
                throw new ArgumentException("Unable to map source '" + source + "' and target '" + target + "'.");

            DeserializeRecursively(mappings, adviceRequester);

            target.CommitChanges();
        }

        private void DeserializeRecursively(IEnumerable<IMapping<XObject, IMemberContext>> mappings, IAdviceRequester adviceRequester)
        {
            foreach (IMapping<XObject, IMemberContext> mapping in mappings)
            {
                IDeserializer deserializer = _recursiveSelector.Select(mapping.Source, mapping.Target, adviceRequester);

                if (deserializer == null && mapping.Source == null)
                    continue;

                if (deserializer == null)
                    throw new ArgumentException("No deserializer could be found for source '" + mapping.Source + "' and target '" + mapping.Target + "'.");

                deserializer.Deserialize(mapping.Source, mapping.Target, adviceRequester);
            }
        }
    }
}