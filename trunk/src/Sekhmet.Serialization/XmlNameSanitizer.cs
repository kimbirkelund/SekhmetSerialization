using System;
using System.Xml;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public static class XmlNameSanitizer
    {
        public static XName SafeToXName(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException("str");

            int index = str.IndexOf('`');

            if (index != -1)
                str = str.Substring(0, index);

            return XmlConvert.EncodeLocalName(str);
        }
    }
}