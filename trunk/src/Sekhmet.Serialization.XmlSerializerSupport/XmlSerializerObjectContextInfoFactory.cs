using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerObjectContextInfoFactory : IObjectContextInfoFactory
    {
        public ObjectContextInfo Create(IObjectContextFactory objectContextFactory, Type type, IAdviceRequester adviceRequester)
        {
            return new ObjectContextInfo(type, type.GetCustomAttributes(true), CreateMemberContextInfos(objectContextFactory, type, adviceRequester));
        }

        private static IEnumerable<MemberContextInfo> CreateMemberContextInfos(IObjectContextFactory objectContextFactory, Type actualType, IAdviceRequester adviceRequester)
        {
            foreach (var propertyInfo in GetRelevantProperties(actualType))
            {
                yield return new MemberContextInfo(
                        propertyInfo.PropertyType,
                        propertyInfo.Name,
                        propertyInfo.GetCustomAttributes(true).ToList(),
                        GetGetter(objectContextFactory, propertyInfo, adviceRequester),
                        GetSetter(propertyInfo));
            }

            foreach (var fieldInfo in GetRelevantFields(actualType))
            {
                yield return new MemberContextInfo(
                        fieldInfo.FieldType,
                        fieldInfo.Name,
                        fieldInfo.GetCustomAttributes(true).ToList(),
                        GetGetter(objectContextFactory, fieldInfo, adviceRequester),
                        GetSetter(fieldInfo));
            }
        }

        private static Func<MemberContext, IObjectContext> GetGetter(IObjectContextFactory objectContextFactory, FieldInfo fieldInfo, IAdviceRequester adviceRequester)
        {
            return c => objectContextFactory.CreateForSerialization(c, fieldInfo.GetValue(c.Target), adviceRequester);
        }

        private static Func<MemberContext, IObjectContext> GetGetter(IObjectContextFactory objectContextFactory, PropertyInfo propertyInfo, IAdviceRequester adviceRequester)
        {
            return c => objectContextFactory.CreateForSerialization(c, propertyInfo.GetValue(c.Target, null), adviceRequester);
        }

        private static bool GetMemberHasAttribute<TAttribute>(IEnumerable<object> attrs, Func<bool> validate)
                where TAttribute : Attribute
        {
            if (attrs.OfType<TAttribute>().FirstOrDefault() == null)
                return false;

            if (!validate())
                return false;

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
                   where GetMemberHasAttribute<XmlAttributeAttribute>(attrs, () => !fieldInfo.IsInitOnly)
                         || GetMemberHasAttribute<XmlElementAttribute>(attrs, () => !fieldInfo.IsInitOnly)
                         || GetMemberHasAttribute<XmlArrayAttribute>(attrs, () => !fieldInfo.IsInitOnly)
                         || GetMemberHasAttribute<XmlArrayItemAttribute>(attrs, () => !fieldInfo.IsInitOnly)
                         || GetMemberHasAttribute<XmlParentAttribute>(attrs, () => !fieldInfo.IsInitOnly)
                   select fieldInfo;
        }

        private static IEnumerable<PropertyInfo> GetRelevantProperties(Type actualType)
        {
            return from propertyInfo in actualType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                   let attrs = propertyInfo.GetCustomAttributes(true)
                   where !propertyInfo.GetIndexParameters().Any()
                   where GetPropertyHasPublicGetterAndSetter(propertyInfo)
                         || GetMemberHasAttribute<XmlAttributeAttribute>(attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                         || GetMemberHasAttribute<XmlElementAttribute>(attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                         || GetMemberHasAttribute<XmlArrayAttribute>(attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                         || GetMemberHasAttribute<XmlArrayItemAttribute>(attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                         || GetMemberHasAttribute<XmlParentAttribute>(attrs, () => propertyInfo.GetGetMethod(true) != null && propertyInfo.GetSetMethod(true) != null)
                   select propertyInfo;
        }

        private static Action<MemberContext, IObjectContext> GetSetter(FieldInfo fieldInfo)
        {
            return (c, v) => fieldInfo.SetValue(c.Target, v.GetObject());
        }

        private static Action<MemberContext, IObjectContext> GetSetter(PropertyInfo propertyInfo)
        {
            return (c, v) => propertyInfo.SetValue(c.Target, v.GetObject(), null);
        }
    }
}