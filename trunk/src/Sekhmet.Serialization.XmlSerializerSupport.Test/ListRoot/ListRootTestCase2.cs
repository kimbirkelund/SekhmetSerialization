using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.ListRoot
{
    public class ArrayRootTestCase2 : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new List<Foo<int>> { new Foo<int> { Value = 1 } };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Sequence",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Foo", new XElement("Value", 1)));
        }
    }
}