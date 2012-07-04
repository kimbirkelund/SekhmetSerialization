using System;
using System.Xml;

namespace Sekhmet.Serialization
{
    public class ValueTypeDeserializerHelper
    {
        public virtual IObjectContext DeserializeFromValue(string value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (String.IsNullOrWhiteSpace(value))
                    return new SimpleValueObjectContext(type, null);

                return new SimpleValueObjectContext(type, Activator.CreateInstance(type, DeserializeFromValue(value, type.GetGenericArguments()[0]).GetObject()));
            }

            if (type == typeof(TimeSpan))
                return DeserializeToTimeSpan(value);

            if (type.IsEnum)
                return DeserializeToEnum(value, type);

            return DeserializeToConvertibleType(value, type);
        }

        protected virtual IObjectContext DeserializeToConvertibleType(string value, Type type)
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

        protected virtual IObjectContext DeserializeToEnum(string value, Type type)
        {
            object enumValue = Enum.Parse(type, value, true);

            return new SimpleValueObjectContext(type, enumValue);
        }

        protected virtual IObjectContext DeserializeToTimeSpan(string value)
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