using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface ISerializerSelector
    {
        ISerializer Select(IMemberContext source, XObject target);
    }
}