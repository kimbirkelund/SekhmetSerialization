using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    partial class BuiltInCollectionsObjectContextFactory
    {
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
    }
}