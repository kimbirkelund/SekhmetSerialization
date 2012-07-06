using System;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CompositeObjectContextFactory : ICompositeObjectContextFactory
    {
        private readonly IList<IObjectContextFactory> _factories;
        private readonly ReadWriteLock _lock = new ReadWriteLock();

        public IEnumerable<IObjectContextFactory> Factories
        {
            get
            {
                using (_lock.EnterReadScope())
                    return _factories.ToList();
            }
        }

        public CompositeObjectContextFactory(IEnumerable<IObjectContextFactory> factories = null)
        {
            _factories = (factories ?? Enumerable.Empty<IObjectContextFactory>()).ToList();
        }

        public void AddFactory(IObjectContextFactory selector, int? index = null)
        {
            using (_lock.EnterWriteScope())
            {
                if (index != null)
                    _factories.Insert(index.Value, selector);
                else
                    _factories.Add(selector);
            }
        }

        public IObjectContext CreateForDeserialization(IMemberContext targetMember, Type targetType)
        {
            return Factories
                .Select(f => f.CreateForDeserialization(targetMember, targetType))
                .Where(d => d != null)
                .FirstOrDefault();
        }

        public IObjectContext CreateForSerialization(IMemberContext sourceMember, object source)
        {
            return Factories
                .Select(f => f.CreateForSerialization(sourceMember, source))
                .Where(d => d != null)
                .FirstOrDefault();
        }

        public void RemoveFactory(IObjectContextFactory selector)
        {
            using (_lock.EnterWriteScope())
                _factories.Remove(selector);
        }
    }
}