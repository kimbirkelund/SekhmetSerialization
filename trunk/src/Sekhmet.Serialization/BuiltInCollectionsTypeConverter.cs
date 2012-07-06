using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logging;

namespace Sekhmet.Serialization
{
    public class BuiltInCollectionsTypeConverter : ITypeConverter
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public Type GetActualType(Type type)
        {
            Type result = null;

            if (type != null)
            {
                if (type != typeof(string))
                {
                    if (!typeof(IDictionary).IsAssignableFrom(type) &&
                        (!type.IsGenericType || type.GetGenericArguments().Length != 2 || !typeof(IDictionary<,>).MakeGenericType(type.GetGenericArguments()).IsAssignableFrom(type)))
                    {
                        if (type.IsClass && typeof(IEnumerable).IsAssignableFrom(type))
                            result = type;
                        else
                        {
                            if (type.IsInterface && type.IsAssignableFrom(typeof(List<object>)))
                                result = typeof(List<object>);
                            else
                            {
                                if (type.IsGenericType && type.GetGenericArguments().Length == 1 && type.IsAssignableFrom(typeof(List<>).MakeGenericType(type.GetGenericArguments())))
                                    result = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                            }
                        }
                    }
                }
            }

            if (_log.IsDebugEnabled)
                _log.Debug("Converted '" + type + "' to '" + result + "'.");

            return result;
        }

        public Type GetActualType(XObject source, IMemberContext target)
        {
            return GetActualType(target.ContractType);
        }
    }
}