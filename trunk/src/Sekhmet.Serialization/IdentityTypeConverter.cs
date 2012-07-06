using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class IdentityTypeConverter : ITypeConverter
    {
        public Type GetActualType(Type type)
        {
            if (type.IsInterface || type.IsAbstract)
                return null;

            return type;
        }

        public Type GetActualType(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            return GetActualType(target.ContractType);
        }
    }
}