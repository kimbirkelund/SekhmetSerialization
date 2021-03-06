﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ListSerializationSupport
{
    public class ListSerializationSupportTestCase : XmlSerializerSerializationTestCaseBase
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
                Bars1 = new List<Bar> {
                                                         new Bar {
                                                                     Value1 = ExpectedBar1Value1,
                                                                     Value2 = ExpectedBar1Value2,
                                                                     Value3 = _expectedBar1Value3,
                                                                     Value4 = _expectedBar1Value4
                                                                 },
                                                         new Bar {
                                                                     Value1 = ExpectedBar2Value1,
                                                                     Value2 = ExpectedBar2Value2,
                                                                     Value3 = _expectedBar2Value3,
                                                                     Value4 = _expectedBar2Value4
                                                                 }
                                                     },
                Bars2 = new List<Bar> {
                                                         new Bar {
                                                                     Value1 = ExpectedBar1Value1,
                                                                     Value2 = ExpectedBar1Value2,
                                                                     Value3 = _expectedBar1Value3,
                                                                     Value4 = _expectedBar1Value4
                                                                 },
                                                         new Bar {
                                                                     Value1 = ExpectedBar2Value1,
                                                                     Value2 = ExpectedBar2Value2,
                                                                     Value3 = _expectedBar2Value3,
                                                                     Value4 = _expectedBar2Value4
                                                                 }
                                                     }
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bars1",
                                             new XElement("Bar",
                                                          new XElement("Value1", ExpectedBar1Value1),
                                                          new XAttribute("Id", ExpectedBar1Value2),
                                                          new XElement("Value3", _expectedBar1Value3),
                                                          new XElement("TimeSpan", _expectedBar1Value4)),
                                             new XElement("Bar",
                                                          new XElement("Value1", ExpectedBar2Value1),
                                                          new XAttribute("Id", ExpectedBar2Value2),
                                                          new XElement("Value3", _expectedBar2Value3),
                                                          new XElement("TimeSpan", _expectedBar2Value4))),
                                new XElement("Bars",
                                             new XElement("SomeBar",
                                                          new XElement("Value1", ExpectedBar1Value1),
                                                          new XAttribute("Id", ExpectedBar1Value2),
                                                          new XElement("Value3", _expectedBar1Value3),
                                                          new XElement("TimeSpan", _expectedBar1Value4)),
                                             new XElement("SomeBar",
                                                          new XElement("Value1", ExpectedBar2Value1),
                                                          new XAttribute("Id", ExpectedBar2Value2),
                                                          new XElement("Value3", _expectedBar2Value3),
                                                          new XElement("TimeSpan", _expectedBar2Value4))));
        }
    }
}