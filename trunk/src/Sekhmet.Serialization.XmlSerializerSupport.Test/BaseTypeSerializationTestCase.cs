using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class BaseTypeSerializationTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedBar1Value1 = "Foo";
        private const string ExpectedBar2Value2 = "24";

        protected override object CreateObject()
        {
            return new FooWithBarBase {
                                          Bar = new Bar2 {
                                                             Value2 = ExpectedBar2Value2
                                                         },
                                          Bars = new List<IBar> {
                                                                    new Bar2 {Value2 = ExpectedBar2Value2},
                                                                    new Bar1 {Value1 = ExpectedBar1Value1}
                                                                }
                                      };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Baz2",
                                             new XAttribute("type", typeof (Bar2).AssemblyQualifiedName),
                                             new XElement("Value2", ExpectedBar2Value2)),
                                new XElement("Bars",
                                             new XElement("Baz2",
                                                          new XAttribute("type", typeof (Bar2).AssemblyQualifiedName),
                                                          new XElement("Value2", ExpectedBar2Value2)),
                                             new XElement("Baz1",
                                                          new XAttribute("type", typeof (Bar1).AssemblyQualifiedName),
                                                          new XElement("Value1", ExpectedBar1Value1))));
        }
    }
}