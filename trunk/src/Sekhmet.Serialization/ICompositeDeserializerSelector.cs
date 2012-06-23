using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface ICompositeDeserializerSelector : IDeserializerSelector
    {
        IEnumerable<IDeserializerSelector> Selectors { get; }

        void AddSelector(IDeserializerSelector selector, int? index = null);
        void RemoveSelector(IDeserializerSelector selector);
    }
}