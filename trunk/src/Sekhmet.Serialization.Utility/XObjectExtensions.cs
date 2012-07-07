using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization.Utility
{
    public static class XObjectExtensions
    {
        public static string ToFriendlyName(this XObject xobj)
        {
            return "[" + xobj.ToCleanFriendlyName() + "]";
        }

        private static IEnumerable<XAttribute> GetRelevantAttributes(XElement xelem)
        {
            return xelem.Attributes()
                .Where(a => a.Name != Constants.XmlSchemaInstanceNamespaceAttribute.Name)
                .Where(a => a.Name != Constants.XmlSchemaNamespaceAttribute.Name);
        }

        private static string ToCleanFriendlyName(this XObject xobj, bool includePrefix = true, bool elementIncludeAttributes = true)
        {
            string prefix = includePrefix && xobj.Parent != null
                                ? xobj.Parent.ToCleanFriendlyName(elementIncludeAttributes: false)
                                : "";

            switch (xobj.NodeType)
            {
                case XmlNodeType.Element:
                    var xelem = (XElement)xobj;
                    IEnumerable<XAttribute> attributes = GetRelevantAttributes(xelem);
                    string attrsStr = elementIncludeAttributes && attributes.Any()
                                          ? "<" + string.Join(", ", attributes.Select(a => a.ToCleanFriendlyName(false)).ToArray()) + ">"
                                          : "";
                    return prefix + "/" + xelem.Name + attrsStr;
                case XmlNodeType.Attribute:
                    var xattr = ((XAttribute)xobj);
                    return prefix + "@" + xattr.Name + "=" + xattr.Value;
                default:
                    return xobj.Parent.ToCleanFriendlyName() + "/" + xobj.NodeType;
            }
        }
    }
}