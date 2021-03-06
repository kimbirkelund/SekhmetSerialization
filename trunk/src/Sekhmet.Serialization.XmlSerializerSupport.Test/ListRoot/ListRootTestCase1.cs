﻿using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ListRoot
{
    public class ArrayRootTestCase1 : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new List<int> { 1, 2, 3 };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Sequence",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Int32", 1),
                                new XElement("Int32", 2),
                                new XElement("Int32", 3));
        }
    }
}