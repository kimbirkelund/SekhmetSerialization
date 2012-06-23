using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    /// <summary>
    ///   Mapper used for collections.
    /// </summary>
    /// <remarks>
    ///   MapForDeserialization: assumes that target contains one member that when SetValue is called adds an element to
    ///   the collection it represents.
    /// 
    ///   MapForSerialization: assumes that the source contains one member for each element in the collection it represents.
    ///   Also each member should have the attributes associated with the member containing the 
    ///   collection. These are needed because they might contain information about what to name
    ///   the elements.
    /// 
    ///   Futhermore it only look at elements, not attributes of the source.
    /// </remarks>
    public class XmlSerializerCollectionMapper : IMapper
    {
        public IEnumerable<IMapping<XObject, IMemberContext>> MapForDeserialization(XElement source, IMemberContext target)
        {
            var targetObject = target.GetValue();
            if (targetObject == null)
                throw new ArgumentException("Target should have had it value set to a collection at this point.", "target");

            var elementNames = GetElementNames(target);

            return source.Elements()
                    .Where(e => elementNames.Contains(e.Name))
                    .Select(elem => new Mapping<XObject, IMemberContext>(elem, targetObject.Members.Single()))
                    .ToList();
        }

        public IEnumerable<IMapping<IMemberContext, XObject>> MapForSerialization(IMemberContext source, XElement target)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var elementType = GetElementType(source.GetActualType());
            if (elementType == null)
                throw new ArgumentException("Collection mapper has been invoked for target that is not a known list type: '" + source.GetActualType() + "'.", "target");

            var sourceObject = source.GetValue();
            if (sourceObject == null)
                return Enumerable.Empty<IMapping<IMemberContext, XObject>>();

            return sourceObject.Members
                    .Select(CreateMapping);
        }

        private static Mapping<IMemberContext, XElement> CreateMapping(IMemberContext m)
        {
            var elem = new XElement(NameElement(m));

            return new Mapping<IMemberContext, XElement>(m, elem);
        }

        private static IEnumerable<XName> GetElementNames(IMemberContext target)
        {
            return GetElementsNamesFromElementAttributes(target)
                   ?? GetElementNamesFromXmlArrayItemAttributes(target)
                   ?? GetElementNamesFromElementType(target);
        }

        private static IEnumerable<XName> GetElementNamesFromElementType(IMemberContext target)
        {
            var elementType = GetElementType(target.ContractType);

            return new XName[] { elementType.Name };
        }

        private static IEnumerable<XName> GetElementNamesFromXmlArrayItemAttributes(IMemberContext target)
        {
            var arrayItemNames = target.Attributes
                    .OfType<XmlArrayItemAttribute>()
                    .Select(a => (XName)a.ElementName)
                    .Where(n => n != null)
                    .ToList();

            if (!arrayItemNames.Any())
                return null;

            return arrayItemNames;
        }

        private static Type GetElementType(Type type)
        {
            var enumInterface = type.GetInterface("System.Collections.Generic.IEnumerable`1");
            if (enumInterface != null)
                return enumInterface.GetGenericArguments()[0];

            if (type.IsSubTypeOf<IEnumerable>())
                return typeof(object);

            return null;
        }

        private static IEnumerable<XName> GetElementsNamesFromElementAttributes(IMemberContext target)
        {
            var elemAttrs = target.Attributes
                    .OfType<XmlElementAttribute>()
                    .ToList();

            if (!elemAttrs.Any())
                return null;

            var elemNames = elemAttrs
                    .Select(a => (XName)a.ElementName)
                    .Where(n => n != null)
                    .ToList();

            if (!elemAttrs.Any())
                return new XName[] { target.Name };

            return elemNames;
        }

        private static XName NameElement(IMemberContext memberContext)
        {
            return NameElementFromElementAttributeExactType(memberContext)
                   ?? NameElementFromElementAttributeInstanceOfType(memberContext)
                   ?? NameElementFromElementAttributeNoType(memberContext)
                   ?? NameElementFromArrayItemAttributeExactType(memberContext)
                   ?? NameElementFromArrayItemAttributeInstanceOfType(memberContext)
                   ?? NameElementFromArrayItemAttributeNoType(memberContext)
                   ?? NameElementFromActualType(memberContext)
                   ?? NameElementFromContractType(memberContext);
        }

        private static XName NameElementFromActualType(IMemberContext memberContext)
        {
            var actualType = memberContext.GetActualType();
            if (actualType == null)
                return null;

            return actualType.Name;
        }

        private static string NameElementFromArrayItemAttributeExactType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlArrayItemAttribute>()
                    .Where(a => a.Type == memberContext.GetActualType())
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }

        private static string NameElementFromArrayItemAttributeInstanceOfType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlArrayItemAttribute>()
                    .Where(a => a.Type != null && memberContext.GetActualType().IsSubTypeOf(a.Type))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }

        private static string NameElementFromArrayItemAttributeNoType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlArrayItemAttribute>()
                    .Where(a => a.Type == null)
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }

        private static XName NameElementFromContractType(IMemberContext memberContext)
        {
            return memberContext.ContractType.Name;
        }

        private static string NameElementFromElementAttributeExactType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type == memberContext.GetActualType())
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }

        private static string NameElementFromElementAttributeInstanceOfType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type != null && memberContext.GetActualType().IsSubTypeOf(a.Type))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }

        private static string NameElementFromElementAttributeNoType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type == null)
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => a.ElementName)
                    .FirstOrDefault();
        }
    }
}