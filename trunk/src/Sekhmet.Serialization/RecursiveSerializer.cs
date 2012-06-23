using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveSerializer : ISerializer
    {
        private readonly IMapper _mapper;
        private readonly ISerializerSelector _recursiveSelector;

        public RecursiveSerializer(IMapper mapper, ISerializerSelector recursiveSelector)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (recursiveSelector == null)
                throw new ArgumentNullException("recursiveSelector");

            _mapper = mapper;
            _recursiveSelector = recursiveSelector;
        }

        public bool Serialize(IMemberContext source, XObject target)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target.NodeType != XmlNodeType.Element)
                throw new ArgumentException("Parameter must be an XML element.", "target");

            var elem = (XElement)target;

            var sourceObject = source.GetValue();

            if (sourceObject == null || sourceObject.GetObject() == null)
            {
                if (XmlSerializerHelper.IsNullable(source, elem))
                {
                    elem.Add(Constants.XsiNilAttribute);
                    return true;
                }

                return false;
            }

            var mappings = _mapper.MapForSerialization(source, elem)
                    .ToList();
            if (mappings == null)
                throw new ArgumentException("Unable to map source '" + source + "' and target '" + target + "'.");

            SerializeRecursively(elem, mappings);

            return true;
        }

        private void SerializeRecursively(XElement targetElement, IEnumerable<IMapping<IMemberContext, XObject>> mappings)
        {
            foreach (var mapping in mappings)
            {
                var serializer = _recursiveSelector.Select(mapping.Source, mapping.Target);
                if (serializer == null)
                    throw new ArgumentException("No serializer could be found for source '" + mapping.Source + "' and target '" + mapping.Target + "'.");

                var shouldAddToTarget = serializer.Serialize(mapping.Source, mapping.Target);

                if (mapping.AddTargetToParent && shouldAddToTarget)
                    targetElement.Add(mapping.Target);
            }
        }
    }
}