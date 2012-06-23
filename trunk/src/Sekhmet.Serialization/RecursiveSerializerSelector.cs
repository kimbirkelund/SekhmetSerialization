using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class RecursiveSerializerSelector : ISerializerSelector
    {
        private readonly ISerializer _serializer;

        public RecursiveSerializerSelector(IMapper mapper, ISerializerSelector recursiveSelector)
        {
            _serializer = new RecursiveSerializer(mapper, recursiveSelector);
        }

        public ISerializer Select(IMemberContext source, XObject target)
        {
            if (target.NodeType != XmlNodeType.Element)
                return null;

            return _serializer;
        }
    }
}