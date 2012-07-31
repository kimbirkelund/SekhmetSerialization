using System.Xml.Linq;
using NUnit.Framework;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlParent
{
    public class XmlParentTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override void AssertCorrectObject(object expected, object actual)
        {
            var actualFoo = (Foo)actual;

            Assert.AreSame(actual, actualFoo.Bar.Parent);
            Assert.AreSame(actual, actualFoo.Bar.PParent);
        }

        protected override object CreateObject()
        {
            var foo = new Foo
            {
                Bar = new Bar()
            };

            foo.Bar.Parent = foo;
            foo.Bar.PParent = foo;

            return foo;
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar"));
        }
    }
}