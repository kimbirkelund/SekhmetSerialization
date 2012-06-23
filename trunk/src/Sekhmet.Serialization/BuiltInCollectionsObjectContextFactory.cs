using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class BuiltInCollectionsObjectContextFactory : IObjectContextFactory
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

        private class DeserializationMemberContext : IMemberContext
        {
            private readonly IList _list;
            private IObjectContext _value;

            public IEnumerable<object> Attributes { get; private set; }
            public Type ContractType { get; private set; }

            public string Name
            {
                get { return "Value"; }
            }

            public DeserializationMemberContext(Type contractType, IEnumerable<object> attributes, IList list)
            {
                if (contractType == null)
                    throw new ArgumentNullException("contractType");
                if (attributes == null)
                    throw new ArgumentNullException("attributes");
                if (list == null)
                    throw new ArgumentNullException("list");

                Attributes = attributes;
                _list = list;
                ContractType = contractType;
            }

            public void CommitChanges() { }

            public IObjectContext GetValue()
            {
                return _value;
            }

            public void SetValue(IObjectContext value)
            {
                _value = value;
                _list.Add(_value.GetObject());
            }
        }

        private class DeserializationMemberContext<T> : IMemberContext
        {
            private readonly ICollection<T> _list;
            private IObjectContext _value;

            public IEnumerable<object> Attributes { get; private set; }
            public Type ContractType { get; private set; }

            public string Name
            {
                get { return "Value"; }
            }

            public DeserializationMemberContext(IEnumerable<object> attributes, IList<T> list)
            {
                if (attributes == null)
                    throw new ArgumentNullException("attributes");
                if (list == null)
                    throw new ArgumentNullException("list");

                Attributes = attributes;
                _list = list;
                ContractType = typeof(T);
            }

            public void CommitChanges() { }

            public IObjectContext GetValue()
            {
                return _value;
            }

            public void SetValue(IObjectContext value)
            {
                _value = value;
                _list.Add((T)_value.GetObject());
            }
        }

        private class DeserializationObjectContext : IObjectContext
        {
            private readonly Type _elementType;
            private readonly object _list;
            private readonly Type _listType;
            public IEnumerable<object> Attributes { get; private set; }

            public IEnumerable<IMemberContext> Members
            {
                get
                {
                    var list = _list as IList;
                    if (list != null)
                        return new[] { new DeserializationMemberContext(_elementType, Attributes, list) };

                    return new[] { (IMemberContext)Activator.CreateInstance(typeof(DeserializationMemberContext<>).MakeGenericType(_elementType), Attributes, _list) };
                }
            }

            public Type Type
            {
                get { return _listType; }
            }

            public DeserializationObjectContext(Type listType, Type elementType, object list, IEnumerable<object> attributes)
            {
                if (elementType == null)
                    throw new ArgumentNullException("elementType");
                if (list == null)
                    throw new ArgumentNullException("list");

                _listType = listType;
                _elementType = elementType;
                _list = list;
                Attributes = (attributes ?? Enumerable.Empty<object>()).ToList();
            }

            public object GetObject()
            {
                if (Type.IsArray)
                    return ToArray(_list);
                return _list;
            }

            private object ToArray(object value)
            {
                return _list.GetType().GetMethod("ToArray").Invoke(value, null);
            }
        }

        private class SerializationMemberContext : IMemberContext
        {
            private readonly Type _contractType;
            private readonly IObjectContext _value;

            public IEnumerable<object> Attributes { get; private set; }

            public Type ContractType
            {
                get { return _contractType; }
            }

            public string Name
            {
                get { return "Add"; }
            }

            public SerializationMemberContext(Type contractType, IObjectContext value, IEnumerable<object> attributes)
            {
                _contractType = contractType;
                _value = value;
                Attributes = attributes;
            }

            public void CommitChanges() { }

            public IObjectContext GetValue()
            {
                return _value;
            }

            public void SetValue(IObjectContext value)
            {
                throw new NotSupportedException();
            }
        }

        private class SerializationObjectContext : IObjectContext
        {
            private readonly Type _elementType;
            private readonly IObjectContextFactory _objectContextFactory;
            private readonly IEnumerable _value;

            public IEnumerable<object> Attributes { get; private set; }

            public IEnumerable<IMemberContext> Members
            {
                get
                {
                    return (from object elem in _value
                            select new SerializationMemberContext(_elementType, _objectContextFactory.CreateForSerialization(null, elem), Attributes));
                }
            }

            public Type Type
            {
                get { return _value.GetType(); }
            }

            public SerializationObjectContext(Type elementType, IEnumerable value, IEnumerable<object> attributes, IObjectContextFactory objectContextFactory)
            {
                if (elementType == null)
                    throw new ArgumentNullException("elementType");
                if (value == null)
                    throw new ArgumentNullException("value");

                _elementType = elementType;
                _value = value;
                Attributes = (attributes ?? Enumerable.Empty<object>()).ToList();
                _objectContextFactory = objectContextFactory;
            }

            public object GetObject()
            {
                return _value;
            }
        }
    }
}