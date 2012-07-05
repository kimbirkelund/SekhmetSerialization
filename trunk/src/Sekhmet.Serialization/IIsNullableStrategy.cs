using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IIsNullableStrategy
    {
        bool IsNullable(IMemberContext source, XElement target);
    }
}