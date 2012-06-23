using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface ICompositeTypeConverter : ITypeConverter
    {
        IEnumerable<ITypeConverter> Converters { get; }

        void AddConverter(ITypeConverter converter, int? index = null);
        void RemoveConverter(ITypeConverter converter);
    }
}