using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public static class ObjectContextInfoFactory
    {
        public static ObjectContextInfo Create(IObjectContextFactory objectContextFactory, Type type)
        {
            return new ObjectContextInfo(type, CreateMemberContextInfos(objectContextFactory, type));
        }

        private static IEnumerable<MemberContextInfo> CreateMemberContextInfos(IObjectContextFactory objectContextFactory, Type actualType)
        {
            foreach (var propertyInfo in GetRelevantProperties(actualType))
            {
                yield return new MemberContextInfo(
                        propertyInfo.PropertyType,
                        propertyInfo.Name,
                        propertyInfo.GetCustomAttributes(true).ToList(),
                        GetGetter(objectContextFactory, propertyInfo),
                        GetSetter(propertyInfo));
            }

            foreach (var fieldInfo in GetRelevantFields(actualType))
            {
                yield return new MemberContextInfo(
                        fieldInfo.FieldType,
                        fieldInfo.Name,
                        fieldInfo.GetCustomAttributes(true).ToList(),
                        GetGetter(objectContextFactory, fieldInfo),
                        GetSetter(fieldInfo));
            }
        }

        private static Func<XmlSerializerMemberContext, IObjectContext> GetGetter(IObjectContextFactory objectContextFactory, FieldInfo fieldInfo)
        {
            return c => objectContextFactory.CreateForSerialization(c, fieldInfo.GetValue(c.Target));
        }

        private static Func<XmlSerializerMemberContext, IObjectContext> GetGetter(IObjectContextFactory objectContextFactory, PropertyInfo propertyInfo)
        {
            return c => objectContextFactory.CreateForSerialization(c, propertyInfo.GetValue(c.Target, null));
        }

        private static bool GetMemberHasAttribute<TAttribute>(Type actualType, MemberInfo memberInfo, IEnumerable<object> attrs, Func<bool> validate)
                where TAttribute : Attribute
        {
            if (attrs.OfType<TAttribute>().FirstOrDefault() == null)
                return false;

            if (!validate())
            {
                return false;
                // TODO: Add logging
                //throw new ArgumentException(
                //        memberInfo.MemberType + " '" + memberInfo.Name + "' of type '" + actualType.FullName + "' has the '" + typeof(TAttribute).Name
                //        + "' attribute but is missing either a getter or setter.", "actualType");
            }

            return true;
        }

        private static bool GetPropertyHasPublicGetterAndSetter(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetGetMethod() != null
                   && propertyInfo.GetGetMethod().IsPublic
                   && propertyInfo.GetSetMethod() != null
                   && propertyInfo.GetSetMethod().IsPublic;
        }

        private static IEnumerable<FieldInfo> GetRelevantFields(Type actualType)
        {
            return from fieldInfo in actualType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                   let attrs = fieldInfo.GetCustomAttributes(true)
                   where !attrs.OfType<XmlIgnoreAttribute>().Any()
                   where GetMemberHasAttribute<XmlAttributeAttribute>(actualType, fieldInfo, attrs, () => !fieldInfo.IsInitOnly)
                         || GetMemberHasAttribute<XmlElementAttribute>(actualType, fieldInfo, attrs, () => !fieldInfo.IsInitOnly)
                   select fieldInfo;
        }

        private static IEnumerable<PropertyInfo> GetRelevantProperties(Type actualType)
        {
            return from propertyInfo in actualType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                   let attrs = propertyInfo.GetCustomAttributes(true)
                   where !propertyInfo.GetIndexParameters().Any()
                   where GetPropertyHasPublicGetterAndSetter(propertyInfo)
                         || GetMemberHasAttribute<XmlAttributeAttribute>(actualType, propertyInfo, attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                         || GetMemberHasAttribute<XmlElementAttribute>(actualType, propertyInfo, attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                   select propertyInfo;
        }

        private static Action<XmlSerializerMemberContext, IObjectContext> GetSetter(FieldInfo fieldInfo)
        {
            return (c, v) => fieldInfo.SetValue(c.Target, v.GetObject());
        }

        private static Action<XmlSerializerMemberContext, IObjectContext> GetSetter(PropertyInfo propertyInfo)
        {
            return (c, v) => propertyInfo.SetValue(c.Target, v.GetObject(), null);
        }
    }
}