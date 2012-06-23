using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveDeserializer : IDeserializer
    {
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

        public void Deserialize(XObject source, IMemberContext target)
        {
            if (source == null)
                return;

            if (source.NodeType != XmlNodeType.Element)
                throw new ArgumentException("Parameter must be an XML element.", "source");

            var elem = (XElement)source;

            var targetType = _typeConverter.GetActualType(source, target);
            if (targetType == null)
                throw new ArgumentException("Unable to get target type for target '" + target + "'.");

            var targetObject = _objectContextFactory.CreateForDeserialization(target, targetType);
            if (targetObject == null)
                throw new ArgumentException("Unable to create target object for target '" + target + "'.");

            target.SetValue(targetObject);

            var mappings = _mapper.MapForDeserialization(elem, target)
                .ToList();
            if (mappings == null)
                throw new ArgumentException("Unable to map source '" + source + "' and target '" + target + "'.");

            DeserializeRecursively(mappings);

            target.CommitChanges();
        }

        private void DeserializeRecursively(IEnumerable<IMapping<XObject, IMemberContext>> mappings)
        {
            foreach (var mapping in mappings)
            {
                var deserializer = _recursiveSelector.Select(mapping.Source, mapping.Target);

                if (deserializer == null && mapping.Source == null)
                    continue;

                if (deserializer == null)
                    throw new ArgumentException("No deserializer could be found for source '" + mapping.Source + "' and target '" + mapping.Target + "'.");

                deserializer.Deserialize(mapping.Source, mapping.Target);
            }
        }
    }
}