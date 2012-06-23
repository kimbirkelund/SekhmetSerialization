using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CompositeTypeConverter : ICompositeTypeConverter
    {
        private readonly IList<ITypeConverter> _converters;
        private readonly ReadWriteLock _lock = new ReadWriteLock();

        public IEnumerable<ITypeConverter> Converters
        {
            get
            {
                using (_lock.EnterReadScope())
                    return _converters.ToList();
            }
        }

        public CompositeTypeConverter(IEnumerable<ITypeConverter> converters = null)
        {
            _converters = (converters ?? Enumerable.Empty<ITypeConverter>()).ToList();
        }

        public void AddConverter(ITypeConverter converter, int? index = null)
        {
            using (_lock.EnterWriteScope())
            {
                if (index != null)
                    _converters.Insert(index.Value, converter);
                else
                    _converters.Add(converter);
            }
        }

        public Type GetActualType(XObject source, IMemberContext target)
        {
            return Converters
                    .Select(c => c.GetActualType(source, target))
                    .Where(t => t != null)
                    .FirstOrDefault();
        }

        public void RemoveConverter(ITypeConverter converter)
        {
            using (_lock.EnterWriteScope())
                _converters.Remove(converter);
        }
    }
}