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
                NotNullableIdWithValue = 42,
                NullableIdWithValue = 24,
                Value = new SimpleSerializable()
            }, actual);
        }


        protected override object CreateObject()
        {
            return new Foo
            {
                NotNullableIdWithValue = 42,
                NullableIdWithValue = 24
            };
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("NotNullableIdWithValue", 42),
                                new XElement("NullableId", Constants.XsiNilAttribute),
                                new XElement("NullableIdWithValue", 24),
                                new XElement("Value", Constants.XsiNilAttribute));
        }
    }
}