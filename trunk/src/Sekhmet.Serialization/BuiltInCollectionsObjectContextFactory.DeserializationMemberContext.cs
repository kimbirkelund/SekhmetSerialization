using System;
using System.Collections;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    partial class BuiltInCollectionsObjectContextFactory
    {
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
    }
}