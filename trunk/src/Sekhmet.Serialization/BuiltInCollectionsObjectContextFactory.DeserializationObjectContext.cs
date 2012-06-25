using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization
{
    partial class BuiltInCollectionsObjectContextFactory
    {
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
    }
}