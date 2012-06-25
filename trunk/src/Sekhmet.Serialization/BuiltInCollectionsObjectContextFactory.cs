using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public partial class BuiltInCollectionsObjectContextFactory : IObjectContextFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IObjectContextFactory _recursionFactory;

        public BuiltInCollectionsObjectContextFactory(IInstantiator instantiator, IObjectContextFactory recursionFactory)
        {
            if (instantiator == null)
                throw new ArgumentNullException("instantiator");
            if (recursionFactory == null)
                throw new ArgumentNullException("recursionFactory");

            _recursionFactory = recursionFactory;
            _instantiator = instantiator;
        }

        public IObjectContext CreateForDeserialization(IMemberContext target, Type actualType)
        {
            if (actualType == null)
                return null;

            var elementType = GetElementTypeForDeserialization(actualType);
            if (elementType == null)
                return null;

            var list = CreateInstance(actualType);

            var attributes = target != null
                                     ? target.Attributes
                                     : Enumerable.Empty<object>();
            attributes = attributes.Concat(actualType.GetCustomAttributes(true));

            return new DeserializationObjectContext(actualType, elementType, list, attributes);
        }

        public IObjectContext CreateForSerialization(IMemberContext source, object value)
        {
            if (value == null)
                return null;

            var elementType = GetElementTypeForSerialization(value.GetType());
            if (elementType == null)
                return null;

            var attributes = source != null
                                     ? source.Attributes
                                     : Enumerable.Empty<object>();
            attributes = attributes.Concat(value.GetType().GetCustomAttributes(true));

            return new SerializationObjectContext(elementType, (IEnumerable)value, attributes, _recursionFactory);
        }

        private object CreateInstance(Type actualType)
        {
            if (actualType.IsArray)
                return _instantiator.Create(typeof(List<>).MakeGenericType(actualType.GetElementType()));

            return _instantiator.Create(actualType);
        }

        private static Type GetElementTypeForDeserialization(Type type)
        {
            var enumInterface = type.GetInterface("System.Collections.Generic.ICollection`1");
            if (enumInterface != null)
                return enumInterface.GetGenericArguments()[0];

            if (type.IsSubTypeOf<IList>())
                return typeof(object);

            if (type.IsArray)
                return type.GetElementType();

            return null;
        }

        private static Type GetElementTypeForSerialization(Type type)
        {
            if (!type.IsSubTypeOf<IEnumerable>())
                return null;

            var enumInterface = type.GetInterface("System.Collections.Generic.IEnumerable`1");
            if (enumInterface != null)
                return enumInterface.GetGenericArguments()[0];

            return typeof(object);
        }
    }
}