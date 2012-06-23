using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class ValueTypeDeserializer : IDeserializer
    {
        private readonly Type _type;

        public ValueTypeDeserializer(Type type)
        {
            _type = type;
        }

        public void Deserialize(XObject source, IMemberContext target)
        {
            if (source == null)
                return;

            IObjectContext targetObject;
            switch (source.NodeType)
            {
                case XmlNodeType.Element:
                    targetObject = DeserializeFromElement((XElement)source, _type);
                    break;
                case XmlNodeType.Attribute:
                    targetObject = DeserializeFromAttribute((XAttribute)source, _type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("source", "Unable to deserialize source of type '" + source.NodeType + "'.");
            }

            target.SetValue(targetObject);

            target.CommitChanges();
        }

        private static IObjectContext DeserializeFromAttribute(XAttribute source, Type type)
        {
            return DeserializeFromValue(source.Value, type);
        }

        private static IObjectContext DeserializeFromElement(XElement source, Type type)
        {
            if (source.HasElements)
                throw new ArgumentException("Cannot deserialize an element which has child-elements.");

            if (HasXsiNilTrueAttribute(source))
                return null;

            return DeserializeFromValue(source.Value, type);
        }

        private static bool HasXsiNilTrueAttribute(XElement source)
        {
            var attr = source.Attribute(Constants.XsiNilAttribute.Name);
            if (attr == null)
                return false;

            bool value;
            bool.TryParse(attr.Value, out value);

            return value;
        }

        private static IObjectContext DeserializeFromValue(string value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrWhiteSpace(value))
                    return new SimpleValueObjectContext(type, null);

                return new SimpleValueObjectContext(type, Activator.CreateInstance(type, DeserializeFromValue(value, type.GetGenericArguments()[0]).GetObject()));
            }

            if (type == typeof(TimeSpan))
                return DeserializeToTimeSpan(value);

            if (type.IsEnum)
                return DeserializeToEnum(value, type);

            return DeserializeToConvertibleType(value, type);
        }

        private static IObjectContext DeserializeToConvertibleType(string value, Type type)
        {
            try
            {
                object result;
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        result = XmlConvert.ToBoolean(value);
                        break;
                    case TypeCode.Char:
                        result = XmlConvert.ToChar(value);
                        break;
                    case TypeCode.SByte:
                        result = XmlConvert.ToSByte(value);
                        break;
                    case TypeCode.Byte:
                        result = XmlConvert.ToByte(value);
                        break;
                    case TypeCode.Int16:
                        result = XmlConvert.ToInt16(value);
                        break;
                    case TypeCode.UInt16:
                        result = XmlConvert.ToUInt16(value);
                        break;
                    case TypeCode.Int32:
                        result = XmlConvert.ToInt32(value);
                        break;
                    case TypeCode.UInt32:
                        result = XmlConvert.ToUInt32(value);
                        break;
                    case TypeCode.Int64:
                        result = XmlConvert.ToInt64(value);
                        break;
                    case TypeCode.UInt64:
                        result = XmlConvert.ToUInt64(value);
                        break;
                    case TypeCode.Single:
                        result = XmlConvert.ToSingle(value);
                        break;
                    case TypeCode.Double:
                        result = XmlConvert.ToDouble(value);
                        break;
                    case TypeCode.Decimal:
                        result = XmlConvert.ToDecimal(value);
                        break;
                    case TypeCode.DateTime:
                        result = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified);
                        break;
                    case TypeCode.String:
                        result = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return new SimpleValueObjectContext(type, result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to deserialize value '" + value + "' as '" + type.Name + "'.", "value", ex);
            }
        }

        private static IObjectContext DeserializeToEnum(string value, Type type)
        {
            var enumValue = Enum.Parse(type, value, true);

            return new SimpleValueObjectContext(type, enumValue);
        }

        private static IObjectContext DeserializeToTimeSpan(string value)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(value, out result))
                return new SimpleValueObjectContext<TimeSpan>(result);

            try
            {
                result = XmlConvert.ToTimeSpan(value);
                return new SimpleValueObjectContext<TimeSpan>(result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to deserialize value '" + value + "' as 'TimeSpan'.", "value", ex);
            }
        }
    }
}