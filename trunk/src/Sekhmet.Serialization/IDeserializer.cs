using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IDeserializer
    {
        void Deserialize(XObject source, IMemberContext target, IAdviceRequester adviceRequester);
    }
}