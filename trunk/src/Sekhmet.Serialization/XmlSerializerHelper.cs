using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    internal class XmlSerializerHelper
    {
        public static bool IsNullable(IMemberContext source, XElement target)
        {
            if (source.ContractType.IsNullable())
                return true;

            var xmlElementAttrs = source.Attributes
                    .OfType<XmlElementAttribute>()
                    .Where(a => a.IsNullable)
                    .Where(a => String.IsNullOrWhiteSpace(a.ElementName) || a.ElementName == target.Name)
                    .ToList();

            var xmlElementAttr = xmlElementAttrs.FirstOrDefault();

            if (xmlElementAttrs.Count > 1)
            {
                xmlElementAttr = xmlElementAttrs
                                         .Where(a => !String.IsNullOrWhiteSpace(a.ElementName))
                                         .FirstOrDefault() ?? xmlElementAttr;
            }

            if (xmlElementAttr == null)
                return false;

            return xmlElementAttr.IsNullable;
        }
    }
}