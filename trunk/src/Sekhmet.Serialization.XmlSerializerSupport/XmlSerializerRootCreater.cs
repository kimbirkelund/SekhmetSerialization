using System;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerRootCreater : IRootCreater
    {
        public XElement CreateRoot(IObjectContext source, IAdviceRequester adviceRequester)
        {
            string name = source.Attributes
                .OfType<XmlRootAttribute>()
                .Select(a => a.ElementName)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(name) && source.Type.IsSubTypeOf<IEnumerable>() && source.Type != typeof(string))
                name = "Sequence";

            if (string.IsNullOrWhiteSpace(name))
                name = source.Type.Name;

            return new XElement(name.SafeToXName(),
                                Constants.XmlSchemaInstanceNamespaceAttribute);
        }

        public void ValidateRoot(XElement source, IObjectContext target, IAdviceRequester adviceRequester)
        {
            string expectedRootName = target.Attributes
                .OfType<XmlRootAttribute>()
                .Select(a => a.ElementName)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(expectedRootName) && target.Type.IsSubTypeOf<IEnumerable>() && target.Type != typeof(string))
                expectedRootName = "Sequence";

            if (string.IsNullOrWhiteSpace(expectedRootName))
                expectedRootName = target.Type.Name;

            if (!CaseInsensitiveXNameComparer.Instance.Equals(source.Name, expectedRootName.SafeToXName()))
                throw new ArgumentException("Root name '" + source.Name + "' did not match expected root name '" + expectedRootName + "'.");
        }

        private XName SanitizeName(string name)
        {
            return XmlConvert.EncodeLocalName(name);
        }
    }
}