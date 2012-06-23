using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface ICompositeSerializerSelector : ISerializerSelector
    {
        IEnumerable<ISerializerSelector> Selectors { get; }

        void AddSelector(ISerializerSelector selector, int? index = null);
        void RemoveSelector(ISerializerSelector selector);
    }
}