using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CompositeSerializerSelector : ICompositeSerializerSelector
    {
        private readonly ReadWriteLock _lock = new ReadWriteLock();
        private readonly IList<ISerializerSelector> _selectors;

        public IEnumerable<ISerializerSelector> Selectors
        {
            get
            {
                using (_lock.EnterReadScope())
                    return _selectors.ToList();
            }
        }

        public CompositeSerializerSelector(IEnumerable<ISerializerSelector> selectors = null)
        {
            _selectors = (selectors ?? Enumerable.Empty<ISerializerSelector>()).ToList();
        }

        public void AddSelector(ISerializerSelector selector, int? index = null)
        {
            using (_lock.EnterWriteScope())
            {
                if (index != null)
                    _selectors.Insert(index.Value, selector);
                else
                    _selectors.Add(selector);
            }
        }

        public void RemoveSelector(ISerializerSelector selector)
        {
            using (_lock.EnterWriteScope())
                _selectors.Remove(selector);
        }

        public ISerializer Select(IMemberContext source, XObject target, IAdviceRequester adviceRequester)
        {
            return Selectors
                    .Select(s => s.Select(source, target, adviceRequester))
                    .Where(d => d != null)
                    .FirstOrDefault();
        }
    }
}