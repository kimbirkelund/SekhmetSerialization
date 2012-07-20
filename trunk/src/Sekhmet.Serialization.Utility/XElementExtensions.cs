using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sekhmet.Serialization.Utility
{
    public static class XElementExtensions
    {
        public static IEnumerable<XElement> Elements(this XElement element, XName name, IComparer<XName> comparer)
        {
            return element.Elements()
                .Where(e => comparer.Compare(name, e.Name) == 0);
        }

        public static XElement Element(this XElement element, XName name, IComparer<XName> comparer)
        {
            return element.Elements()
                .Where(e => comparer.Compare(name, e.Name) == 0)
                .FirstOrDefault();
        }

        public static IEnumerable<XAttribute> Attributes(this XElement element, XName name, IComparer<XName> comparer)
        {
            return element.Attributes()
                .Where(e => comparer.Compare(name, e.Name) == 0);
        }

        public static XAttribute Attribute(this XElement element, XName name, IComparer<XName> comparer)
        {
            return element.Attributes()
                .Where(e => comparer.Compare(name, e.Name) == 0)
                .FirstOrDefault();
        }
    }
}
