using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class ValueTypeDeserializerSelector : IDeserializerSelector
    {
        private readonly IDictionary<Type, ValueTypeDeserializer> _deserializers = new Dictionary<Type, ValueTypeDeserializer>();
        private readonly ReadWriteLock _lock = new ReadWriteLock();

        public virtual IDeserializer Select(XObject source, IMemberContext target)
        {
            return Select(source, target, target.ContractType);
        }

        protected IDeserializer GetDeserializer(Type contractType)
        {
            ValueTypeDeserializer deserializer;
            using (_lock.EnterReadScope())
                _deserializers.TryGetValue(contractType, out deserializer);

            if (deserializer == null)
            {
                using (_lock.EnterWriteScope())
                {
                    if (!_deserializers.TryGetValue(contractType, out deserializer))
                        _deserializers[contractType] = deserializer = new ValueTypeDeserializer(contractType);
                }
            }

            return deserializer;
        }

        protected virtual IDeserializer Select(XObject source, IMemberContext target, Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return null;
                case TypeCode.Object:
                    if (target.ContractType == typeof(TimeSpan))
                        break;
                    if (target.ContractType == typeof(string))
                        break;
                    if (target.ContractType.IsNullable())
                        break;

                    return null;
            }

            if (source == null)
                return GetDeserializer(type);

            if (source.NodeType == XmlNodeType.Element && !((XElement)source).HasElements)
                return GetDeserializer(type);

            if (source.NodeType == XmlNodeType.Attribute)
                return GetDeserializer(type);

            return null;
        }
    }
}