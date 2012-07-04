using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class DefaultIsNullableStrategy : IIsNullableStrategy
    {
        public virtual bool IsNullable(IMemberContext source, XElement target)
        {
            return source.ContractType.IsNullable();
        }
    }
}