using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class BuiltInCollectionsTypeConverter : ITypeConverter
    {
        public Type GetActualType(Type type)
        {
            if (type == null)
                return null;

            if (type == typeof(string))
                return null;

            if (typeof(IDictionary).IsAssignableFrom(type)
                || (type.IsGenericType && type.GetGenericArguments().Length == 2 && typeof(IDictionary<,>).MakeGenericType(type.GetGenericArguments()).IsAssignableFrom(type)))
                return null;

            if (type.IsClass && typeof(IEnumerable).IsAssignableFrom(type))
                return type;

            if (type.IsInterface && type.IsAssignableFrom(typeof(List<object>)))
                return typeof(List<object>);

            if (type.IsGenericType && type.GetGenericArguments().Length == 1 && type.IsAssignableFrom(typeof(List<>).MakeGenericType(type.GetGenericArguments())))
                return typeof(List<>).MakeGenericType(type.GetGenericArguments());

            return null;
        }

        public Type GetActualType(XObject source, IMemberContext target)
        {
            return GetActualType(target.ContractType);
        }
    }
}