using System;
using System.Collections.Generic;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class CachingObjectContextFactory : IObjectContextFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ReadWriteLock _lock = new ReadWriteLock();
        private readonly IDictionary<Type, ObjectContextInfo> _mapActualTypeToContextInfo = new Dictionary<Type, ObjectContextInfo>();
        private readonly IObjectContextInfoFactory _objectContextInfoFactory;
        private readonly IObjectContextFactory _recursionFactory;

        public CachingObjectContextFactory(IInstantiator instantiator, IObjectContextInfoFactory objectContextInfoFactory, IObjectContextFactory recursionFactory)
        {
            if (instantiator == null)
                throw new ArgumentNullException("instantiator");
            if (objectContextInfoFactory == null)
                throw new ArgumentNullException("objectContextInfoFactory");
            if (recursionFactory == null)
                throw new ArgumentNullException("recursionFactory");

            _instantiator = instantiator;
            _objectContextInfoFactory = objectContextInfoFactory;
            _recursionFactory = recursionFactory;
        }

        public IObjectContext CreateForDeserialization(IMemberContext targetMember, Type targetType)
        {
            var contextInfo = GetContextInfo(targetType);

            var target = _instantiator.Create(targetType);
            if (target == null)
                throw new ArgumentException("Unable to create instance of '" + targetType + "' for member '" + targetMember + "'.");

            return contextInfo.CreateFor(target);
        }

        public IObjectContext CreateForSerialization(IMemberContext sourceMember, object source)
        {
            if (source == null)
                return null;

            var contextInfo = GetContextInfo(source.GetType());

            return contextInfo.CreateFor(source);
        }

        private ObjectContextInfo CreateContextInfo(Type actualType)
        {
            return _objectContextInfoFactory.Create(_recursionFactory, actualType);
        }

        private ObjectContextInfo GetContextInfo(Type actualType)
        {
            ObjectContextInfo contextInfo;
            using (_lock.EnterReadScope())
                _mapActualTypeToContextInfo.TryGetValue(actualType, out contextInfo);

            if (contextInfo == null)
            {
                using (_lock.EnterWriteScope())
                {
                    if (!_mapActualTypeToContextInfo.TryGetValue(actualType, out contextInfo))
                        _mapActualTypeToContextInfo[actualType] = contextInfo = CreateContextInfo(actualType);
                }
            }

            return contextInfo;
        }
    }
}