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

        private static string ToCleanFriendlyName(this XObject xobj, bool includePrefix = true, bool elementIncludeAttributes = true)
        {
            string prefix = includePrefix && xobj.Parent != null
                                ? xobj.Parent.ToCleanFriendlyName(elementIncludeAttributes: false)
                                : "";

            switch (xobj.NodeType)
            {
                case XmlNodeType.Element:
                    var xelem = (XElement)xobj;
                    string attrsStr = elementIncludeAttributes && xelem.Attributes().Any()
                                          ? "<" + string.Join(", ", xelem.Attributes().Select(a => a.ToCleanFriendlyName(false)).ToArray()) + ">"
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