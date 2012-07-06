using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveSerializerSelector : ISerializerSelector
    {
        private readonly ISerializer _serializer;

        public RecursiveSerializerSelector(IMapper mapper, ISerializerSelector recursiveSelector, IIsNullableStrategy isNullableStrategy = null)
        {
            _serializer = new RecursiveSerializer(mapper, recursiveSelector, isNullableStrategy);
        }

        public ISerializer Select(IMemberContext source, XObject target, IAdviceRequester adviceRequester)
        {
            if (target.NodeType != XmlNodeType.Element)
                return null;

            return _serializer;
        }
    }
}