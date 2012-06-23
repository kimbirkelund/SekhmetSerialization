using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CompositeDeserializerSelector : ICompositeDeserializerSelector
    {
        private readonly ReadWriteLock _lock = new ReadWriteLock();
        private readonly IList<IDeserializerSelector> _selectors;

        public IEnumerable<IDeserializerSelector> Selectors
        {
            get
            {
                using (_lock.EnterReadScope())
                    return _selectors.ToList();
            }
        }

        public CompositeDeserializerSelector(IEnumerable<IDeserializerSelector> selectors = null)
        {
            _selectors = (selectors ?? Enumerable.Empty<IDeserializerSelector>()).ToList();
        }

        public void AddSelector(IDeserializerSelector selector, int? index = null)
        {
            using (_lock.EnterWriteScope())
            {
                if (index != null)
                    _selectors.Insert(index.Value, selector);
                else
                    _selectors.Add(selector);
            }
        }

        public void RemoveSelector(IDeserializerSelector selector)
        {
            using (_lock.EnterWriteScope())
                _selectors.Remove(selector);
        }

        public IDeserializer Select(XObject source, IMemberContext target)
        {
            return Selectors
                    .Select(s => s.Select(source, target))
                    .Where(d => d != null)
                    .FirstOrDefault();
        }
    }
}