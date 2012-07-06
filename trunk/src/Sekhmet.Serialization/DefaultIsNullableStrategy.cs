using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class DefaultIsNullableStrategy : IIsNullableStrategy
    {
        public virtual bool IsNullable(IMemberContext source, XElement target, IAdviceRequester adviceRequester)
        {
            return source.ContractType.IsNullable();
        }
    }
}