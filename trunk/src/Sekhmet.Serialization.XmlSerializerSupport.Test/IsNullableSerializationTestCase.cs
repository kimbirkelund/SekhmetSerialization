using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class IsNullableSerializationTestCase : XmlSerializerSerializationTestCaseBase
    {
        public override void AssertCorrectObject(object actual)
        {
            AssertCorrectObject(new FooWithIsNullable
            {
                Value = new SimpleSerializable()
            }, actual);
        }


        protected override object CreateObject()
        {
            return new FooWithIsNullable();
        }

        protected override XElement CreateXml()
        {
            return new XElement("FooWithIsNullable",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Value", Constants.XsiNilAttribute));
        }
    }
}