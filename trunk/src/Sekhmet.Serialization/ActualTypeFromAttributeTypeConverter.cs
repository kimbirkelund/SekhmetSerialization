using System;
using System.Xml;
using System.Xml.Linq;
using Common.Logging;

namespace Sekhmet.Serialization
{
    public class ActualTypeFromAttributeTypeConverter : ITypeConverter
    {
        public static readonly XName DefaultTypeAttributeName = "type";

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private readonly XName _attributeName;

        public ActualTypeFromAttributeTypeConverter(XName attributeName = null)
        {
            _attributeName = attributeName ?? DefaultTypeAttributeName;
        }

        public Type GetActualType(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.NodeType != XmlNodeType.Element)
                return null;

            var elem = (XElement) source;

            XAttribute attr = elem.Attribute(_attributeName);
            if (attr == null || string.IsNullOrWhiteSpace(attr.Value))
                return null;

            Type result = Type.GetType(attr.Value);

            LogResult(result, attr);

            return result;
        }

        private static void LogResult(Type result, XAttribute attr)
        {
            if (result == null)
                _log.Warn("Unable to load type from attribute named '" + attr.Name + "' with value '" + attr.Value + "'.");
            else if (_log.IsDebugEnabled)
                _log.Debug("Loaded type '" + result + "' from attribute named '" + attr.Name + "'.");
        }
    }
}