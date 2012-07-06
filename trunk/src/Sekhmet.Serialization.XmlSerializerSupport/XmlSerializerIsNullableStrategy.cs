using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerIsNullableStrategy : DefaultIsNullableStrategy
    {
        public override bool IsNullable(IMemberContext source, XElement target, IAdviceRequester adviceRequester)
        {
            if (base.IsNullable(source, target, adviceRequester))
                return true;

            List<XmlElementAttribute> xmlElementAttrs = source.Attributes
                .OfType<XmlElementAttribute>()
                .Where(a => a.IsNullable)
                .Where(a => string.IsNullOrWhiteSpace(a.ElementName) || a.ElementName == target.Name)
                .ToList();

            XmlElementAttribute xmlElementAttr = xmlElementAttrs.FirstOrDefault();

            if (xmlElementAttrs.Count > 1)
            {
                xmlElementAttr = xmlElementAttrs
                                     .Where(a => !string.IsNullOrWhiteSpace(a.ElementName))
                                     .FirstOrDefault() ?? xmlElementAttr;
            }

            if (xmlElementAttr == null)
                return false;

            return xmlElementAttr.IsNullable;
        }
    }
}