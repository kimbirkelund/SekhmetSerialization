﻿using System;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.NonPublicMembersSerialization
{
    public class ArraySerializationSupportTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedBar1Value1 = "Foo";
        private const int ExpectedBar1Value2 = 42;
        private const string ExpectedBar2Value1 = "Foo2";
        private const int ExpectedBar2Value2 = 24;

        private readonly DateTime _expectedBar1Value3 = new DateTime(2012, 3, 27, 10, 34, 59);
        private readonly TimeSpan _expectedBar1Value4 = new TimeSpan(1, 12, 34, 42);
        private readonly DateTime _expectedBar2Value3 = new DateTime(2011, 2, 26, 9, 33, 58);
        private readonly TimeSpan _expectedBar2Value4 = new TimeSpan(11, 33, 24);

        protected override object CreateObject()
        {
            return new Foo
            {
                PBars2 = new[] {
                                                 new Bar {
                                                             PValue2 = ExpectedBar1Value2,
                                                             PValue4 = _expectedBar1Value4
                                                         },
                                                 new Bar {
                                                             PValue2 = ExpectedBar2Value2,
                                                             PValue4 = _expectedBar2Value4
                                                         }
                                             }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bars",
                                             new XElement("SomeBar",
                                                          new XAttribute("Id", ExpectedBar1Value2),
                                                          new XElement("TimeSpan", _expectedBar1Value4)),
                                             new XElement("SomeBar",
                                                          new XAttribute("Id", ExpectedBar2Value2),
                                                          new XElement("TimeSpan", _expectedBar2Value4))));
        }
    }
}