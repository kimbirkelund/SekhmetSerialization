using System;
using System.Xml;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class ValueTypeSerializerSelector : ISerializerSelector
    {
        private readonly ValueTypeSerializer _serializer = new ValueTypeSerializer();

        public ISerializer Select(IMemberContext source, XObject target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                return null;

            var actualType = source.GetActualType() ?? source.ContractType;
            if (actualType == null)
                throw new ArgumentException("Unable to get actual type for '" + source + "'.", "source");

            var typeCode = Type.GetTypeCode(actualType);

            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return null;
                case TypeCode.Object:
                    if (source.ContractType == typeof(TimeSpan))
                        break;
                    if (source.ContractType == typeof(string))
                        break;
                    if (source.ContractType.IsNullable())
                        break;

                    return null;
            }

            if (target.NodeType != XmlNodeType.Element && target.NodeType != XmlNodeType.Attribute)
                return null;

            return _serializer;
        }
    }
}