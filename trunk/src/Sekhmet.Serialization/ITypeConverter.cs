using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface ITypeConverter
    {
        Type GetActualType(XObject source, IMemberContext target);
    }
}