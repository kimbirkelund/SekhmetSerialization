using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IDeserializerSelector
    {
        IDeserializer Select(XObject source, IMemberContext target, IAdviceRequester adviceRequester);
    }
}