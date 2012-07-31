using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerMapper : IMapper
    {
        public IEnumerable<IMapping<XObject, IMemberContext>> MapForDeserialization(XElement source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            var targetObject = target.GetValue();
            foreach (var member in targetObject.Members.Where(m => !m.Attributes.OfType<XmlIgnoreAttribute>().Any()))
            {
                if (member.Attributes.OfType<XmlParentAttribute>().Any())
                    continue;

                var sourceType = GetSourceTypeAndPotentialNames(member);

                switch (sourceType.Key)
                {
                    case XmlNodeType.Element:
                        foreach (var mapping in GetMappingsFromElement(sourceType.Value, source, targetObject, member, adviceRequester))
                            yield return mapping;
                        break;
                    case XmlNodeType.Attribute:
                        yield return GetMappingFromAttribute(sourceType.Value, source, member);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public IEnumerable<IMapping<IMemberContext, XObject>> MapForSerialization(IMemberContext source, XElement target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            var sourceObject = source.GetValue();
            if (sourceObject == null)
                return Enumerable.Empty<IMapping<IMemberContext, XObject>>();

            return sourceObject.Members
                    .Where(m => !m.Attributes.OfType<XmlIgnoreAttribute>().Any())
                    .Where(m => !m.Attributes.OfType<XmlParentAttribute>().Any())
                    .Select(member => CreateMappingForSerialization(member, target))
                    .ToList();
        }

        private static IMapping<IMemberContext, XObject> CreateMappingForSerialization(IMemberContext member, XElement target)
        {
            var targetTypeAndName = GetTargetTypeAndName(member);

            XObject xobj;
            bool addTargetToParent = true;
            switch (targetTypeAndName.Key)
            {
                case XmlNodeType.Element:
                    if (!member.ContractType.IsSubTypeOf<IXmlSerializable>() && member.ContractType.IsCollectionType() && HasXmlElementAttribute(member))
                    {
                        xobj = target;
                        addTargetToParent = false;
                    }
                    else
                        xobj = new XElement(targetTypeAndName.Value);
                    break;
                case XmlNodeType.Attribute:
                    xobj = new XAttribute(targetTypeAndName.Value, string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Mapped to an unknown node type: " + targetTypeAndName.Key + ".");
            }

            return new Mapping<IMemberContext, XObject>(member, xobj, addTargetToParent);
        }

        private static object GetActualObject(IMemberContext memberContext)
        {
            var objectContextValue = memberContext.GetValue();
            if (objectContextValue == null)
                return null;

            var objectContextValueObject = objectContextValue.GetObject();
            return objectContextValueObject;
        }

        private static Type GetActualType(IMemberContext memberContext)
        {
            var objectContextValueObject = GetActualObject(memberContext);
            if (objectContextValueObject == null)
                return null;

            return objectContextValueObject.GetType();
        }

        private static IMapping<XObject, IMemberContext> GetMappingFromAttribute(IEnumerable<string> potentialNames, XElement source, IMemberContext member)
        {
            var attr = potentialNames
                    .Select(name => source.Attribute(name, CaseInsensitiveXNameComparer.Instance))
                    .Where(a => a != null)
                    .FirstOrDefault();

            return new Mapping<XObject, IMemberContext>(attr, member);
        }

        private static IEnumerable<IMapping<XElement, IMemberContext>> GetMappingsFromElement(IEnumerable<string> potentialNames, XElement source, IObjectContext targetOwner, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (!target.ContractType.IsSubTypeOf<IXmlSerializable>() && target.ContractType.IsCollectionType() && HasXmlElementAttribute(target))
                yield return new Mapping<XElement, IMemberContext>(source, target);
            else
            {
                var elems = potentialNames
                        .Distinct()
                        .SelectMany(name => source.Elements(name, CaseInsensitiveXNameComparer.Instance))
                        .Where(e => e != null)
                        .ToList();

                var elem = elems.FirstOrDefault();
                if (elems.Count > 1)
                    elem = RequestAdviceForMultipleMatches(adviceRequester, source, targetOwner, target, elems, elems.First());

                yield return new Mapping<XElement, IMemberContext>(elem, target);
            }

            foreach (var mapping in GetMappingsFromElementForXmlChoiceIdentifierAttribute(potentialNames, source, targetOwner, target))
                yield return mapping;
        }

        private static XElement RequestAdviceForMultipleMatches(IAdviceRequester adviceRequester, XElement source, IObjectContext targetOwner, IMemberContext target, IEnumerable<XElement> matches, XElement selectedMatch)
        {
            var args = new Advicing.MultipleMatchesAdviceRequestedEventArgs(source, targetOwner, target, matches, selectedMatch);

            adviceRequester.RequestAdvice(args);

            return args.SelectedMatch as XElement;
        }

        private static IEnumerable<IMapping<XElement, IMemberContext>> GetMappingsFromElementForXmlChoiceIdentifierAttribute(IEnumerable<string> potentialNames, XElement source, IObjectContext targetOwner, IMemberContext target)
        {
            var xmlChoiceIdentifierAttr = target.Attributes
                    .OfType<XmlChoiceIdentifierAttribute>()
                    .FirstOrDefault();

            if (xmlChoiceIdentifierAttr == null)
                yield break;

            var choiceIdentifier = targetOwner.Members
                    .Where(m => m.Name == xmlChoiceIdentifierAttr.MemberName)
                    .SingleOrDefault();

            if (choiceIdentifier == null)
                throw new ArgumentException("XmlChoiceIdentifierAttribute on '" + target + "' specified member '" + xmlChoiceIdentifierAttr.MemberName + "' that does not exist on object '"
                                            + targetOwner + "'.");

            if (choiceIdentifier.ContractType.IsCollectionType())
            {
                var newTarget = new XElement(choiceIdentifier.Name);
                foreach (var elem in potentialNames.SelectMany(n => source.Elements(n)))
                    newTarget.Add(new XElement("Value", elem.Name));

                yield return new Mapping<XElement, IMemberContext>(newTarget, choiceIdentifier);
            }
            else
            {
                var elem = potentialNames
                        .Select(name => source.Element(name))
                        .Where(e => e != null)
                        .FirstOrDefault();
                if (elem == null)
                    yield break;

                yield return new Mapping<XElement, IMemberContext>(new XElement(choiceIdentifier.Name, elem.Name), choiceIdentifier);
            }
        }

        private static KeyValuePair<XmlNodeType, IEnumerable<string>> GetSourceTypeAndPotentialNames(IMemberContext member)
        {
            var attrAttrs = member.Attributes
                    .OfType<XmlAttributeAttribute>()
                    .ToList();
            if (attrAttrs.Any())
                return GetSourceTypeAndPotentialNames(member, XmlNodeType.Attribute, attrAttrs.Select(a => a.AttributeName).Where(n => !string.IsNullOrWhiteSpace(n)));

            var elemAttrs = member.Attributes
                    .OfType<XmlElementAttribute>()
                    .ToList();
            if (elemAttrs.Any())
                return GetSourceTypeAndPotentialNames(member, XmlNodeType.Element, elemAttrs.Select(a => a.ElementName).Where(n => !string.IsNullOrWhiteSpace(n)));

            var arrayAttrs = member.Attributes
                    .OfType<XmlArrayAttribute>()
                    .ToList();
            if (arrayAttrs.Any())
                return GetSourceTypeAndPotentialNames(member, XmlNodeType.Element, arrayAttrs.Select(a => a.ElementName).Where(n => !string.IsNullOrWhiteSpace(n)));

            return new KeyValuePair<XmlNodeType, IEnumerable<string>>(XmlNodeType.Element, new[] { member.Name });
        }

        private static KeyValuePair<XmlNodeType, IEnumerable<string>> GetSourceTypeAndPotentialNames(IMemberContext member, XmlNodeType type, IEnumerable<string> potentialNames)
        {
            if (!potentialNames.Any())
                potentialNames = new[] { member.Name };

            return new KeyValuePair<XmlNodeType, IEnumerable<string>>(type, potentialNames
                                                                                    .Where(n => !string.IsNullOrWhiteSpace(n)));
        }

        private static KeyValuePair<XmlNodeType, XName> GetTargetTypeAndName(IMemberContext memberContext)
        {
            return NameElementFromExactTypeMatch(memberContext)
                   ?? NameElementFromInstanceOfTypeMatch(memberContext)
                   ?? NameElementFromNoType(memberContext)
                   ?? NameArrayElementFromNoType(memberContext)
                   ?? NameAttributeFromExactTypeMatch(memberContext)
                   ?? NameAttributeFromInstanceOfTypeMatch(memberContext)
                   ?? NameAttributeFromNoType(memberContext)
                   ?? NameElementFromMemberName(memberContext);
        }

        private static bool HasXmlElementAttribute(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>().Any();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameArrayElementFromNoType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlArrayAttribute>()
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Element, a.ElementName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameAttributeFromExactTypeMatch(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlAttributeAttribute>()
                    .Where(a => a.Type == GetActualType(memberContext))
                    .Where(a => !string.IsNullOrWhiteSpace(a.AttributeName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Attribute, a.AttributeName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameAttributeFromInstanceOfTypeMatch(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlAttributeAttribute>()
                    .Where(a => a.Type != null && a.Type.IsInstanceOfType(GetActualObject(memberContext)))
                    .Where(a => !string.IsNullOrWhiteSpace(a.AttributeName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Attribute, a.AttributeName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameAttributeFromNoType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlAttributeAttribute>()
                    .Where(a => a.Type == null)
                    .Where(a => !string.IsNullOrWhiteSpace(a.AttributeName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Attribute, a.AttributeName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameElementFromExactTypeMatch(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type == GetActualType(memberContext))
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Element, a.ElementName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName>? NameElementFromInstanceOfTypeMatch(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type != null && a.Type.IsInstanceOfType(GetActualObject(memberContext)))
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Element, a.ElementName))
                    .FirstOrDefault();
        }

        private static KeyValuePair<XmlNodeType, XName> NameElementFromMemberName(IMemberContext memberContext)
        {
            return new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Element, memberContext.Name);
        }

        private static KeyValuePair<XmlNodeType, XName>? NameElementFromNoType(IMemberContext memberContext)
        {
            return memberContext.Attributes.OfType<XmlElementAttribute>()
                    .Where(a => a.Type == null)
                    .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                    .Select(a => (KeyValuePair<XmlNodeType, XName>?)new KeyValuePair<XmlNodeType, XName>(XmlNodeType.Element, a.ElementName))
                    .FirstOrDefault();
        }
    }
}