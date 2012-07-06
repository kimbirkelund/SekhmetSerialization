using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization
{
    partial class BuiltInCollectionsObjectContextFactory
    {
        private class SerializationObjectContext : IObjectContext
        {
            private readonly IAdviceRequester _adviceRequester;
            private readonly Type _elementType;
            private readonly IObjectContextFactory _objectContextFactory;
            private readonly IEnumerable _value;

            public SerializationObjectContext(Type elementType, IEnumerable value, IEnumerable<object> attributes, IObjectContextFactory objectContextFactory, IAdviceRequester adviceRequester)
            {
                if (elementType == null)
                    throw new ArgumentNullException("elementType");
                if (value == null)
                    throw new ArgumentNullException("value");

                _elementType = elementType;
                _value = value;
                Attributes = (attributes ?? Enumerable.Empty<object>()).ToList();
                _objectContextFactory = objectContextFactory;
                _adviceRequester = adviceRequester;
            }

            public IEnumerable<object> Attributes { get; private set; }

            public object GetObject()
            {
                return _value;
            }

            public IEnumerable<IMemberContext> Members
            {
                get
                {
                    return (from object elem in _value
                            select new SerializationMemberContext(_elementType, _objectContextFactory.CreateForSerialization(null, elem, _adviceRequester), Attributes));
                }
            }

            public Type Type
            {
                get { return _value.GetType(); }
            }
        }
    }
}