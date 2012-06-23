using System;
using System.Collections.Generic;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class SimpleValueObjectContext : IObjectContext
    {
        private readonly Type _type;
        private readonly object _value;

        public Type Type
        {
            get { return _type; }
        }

        public IEnumerable<object> Attributes
        {
            get { yield break; }
        }

        public Type ContractType
        {
            get { return _type; }
        }

        public IEnumerable<IMemberContext> Members
        {
            get { yield break; }
        }

        public SimpleValueObjectContext(Type type, object value)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (value == null && type.IsValueType && !type.IsNullable())
                throw new ArgumentNullException("Specified type '" + type.Name + "' cannot be null.", "value");

            if (value != null && !type.IsInstanceOfType(value))
                throw new ArgumentException("Specified value '" + value + "' is not an instance of the specified type '" + type + "'.", "value");

            _value = value;
            _type = type;
        }

        public object GetObject()
        {
            return _value;
        }
    }

    public class SimpleValueObjectContext<TValue> : SimpleValueObjectContext
    {
        public SimpleValueObjectContext(TValue value)
            : base(typeof(TValue), value) { }
    }
}