using System.Xml.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.IsNullableSerialization
{
    public class IsNullableSerializationTestCase : XmlSerializerSerializationTestCaseBase
    {
        public override void AssertCorrectObject(object actual)
        {
            AssertCorrectObject(new Foo
            {
                Value = new SimpleSerializable()
            }, actual);
        }


        protected override object CreateObject()
        {
            return new Foo();
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value", Constants.XsiNilAttribute));
        }
    }
}