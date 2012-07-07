﻿using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerRootCreater : IRootCreater
    {
        public XElement CreateRoot(IObjectContext source, IAdviceRequester adviceRequester)
        {
            return new XElement(source.Attributes
                                        .OfType<XmlRootAttribute>()
                                        .Select(a => a.ElementName)
                                        .Where(n => !string.IsNullOrWhiteSpace(n))
                                        .FirstOrDefault() ?? source.Type.Name,
                                    Constants.XmlSchemaInstanceNamespaceAttribute);
        }

        public void ValidateRoot(XElement source, IObjectContext target, IAdviceRequester adviceRequester)
        {
            var expectedRootName = target.Attributes
                                        .OfType<XmlRootAttribute>()
                                        .Select(a => a.ElementName)
                                        .Where(n => !string.IsNullOrWhiteSpace(n))
                                        .FirstOrDefault() ?? target.Type.Name;

            if (source.Name != expectedRootName)
                throw new ArgumentException("Root name '" + source.Name + "' did not match expected root name '" + expectedRootName + "'.");
        }
    }
}