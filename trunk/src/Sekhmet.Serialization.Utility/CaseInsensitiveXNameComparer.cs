using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sekhmet.Serialization.Utility
{
    public class CaseInsensitiveXNameComparer : IComparer<XName>, IEqualityComparer<XName>
    {
        public static readonly CaseInsensitiveXNameComparer Instance = new CaseInsensitiveXNameComparer();

        public int Compare(XName x, XName y)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(x.ToString(), y.ToString());
        }

        public bool Equals(XName x, XName y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(XName obj)
        {
            return obj.GetHashCode();
        }
    }
}
