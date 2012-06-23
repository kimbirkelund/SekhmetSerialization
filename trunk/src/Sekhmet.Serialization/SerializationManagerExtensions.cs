using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public static class SerializationManagerExtensions
    {
        public static T Deserialize<T>(this ISerializationManager manager, XElement source)
        {
            return (T)manager.Deserialize(source, typeof(T));
        }

        public static object DeserializeFromLocation(this ISerializationManager manager, Uri source, Type targetType)
        {
            var elem = XElement.Load(source.ToString());

            return manager.Deserialize(elem, targetType);
        }

        public static T DeserializeFromLocation<T>(this ISerializationManager manager, Uri source)
        {
            return (T)manager.DeserializeFromLocation(source, typeof(T));
        }

        public static object DeserializeFromLocation(this ISerializationManager manager, string source, Type targetType)
        {
            var elem = XElement.Load(source);

            return manager.Deserialize(elem, targetType);
        }

        public static T DeserializeFromLocation<T>(this ISerializationManager manager, string source)
        {
            return (T)manager.DeserializeFromLocation(source, typeof(T));
        }

        public static object DeserializeFromString(this ISerializationManager manager, string source, Type targetType)
        {
            var elem = XElement.Parse(source);

            return manager.Deserialize(elem, targetType);
        }

        public static T DeserializeFromString<T>(this ISerializationManager manager, string source)
        {
            return (T)manager.DeserializeFromString(source, typeof(T));
        }

        public static string SerializeToString(this ISerializationManager manager, object source)
        {
            return manager.Serialize(source).ToString();
        }
    }
}