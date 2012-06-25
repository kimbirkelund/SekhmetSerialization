using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class IsNullableTest
    {
        [Test]
        public void TestDeserialize1()
        {
            var serialier = new XmlSerializer(typeof (FooWithIsNullable));

            var elem = new XElement("FooWithIsNullable");

            var foo = (FooWithIsNullable) serialier.Deserialize(elem.CreateReader());
            Console.WriteLine(elem);

            Assert.Null(foo.Value);
        }

        [Test]
        public void TestDeserialize2()
        {
            var serialier = new XmlSerializer(typeof (FooWithIsNullable));

            var elem = new XElement("FooWithIsNullable",
                                    Constants.XmlSchemaInstanceNamespaceAttribute,
                                    Constants.XmlSchemaNamespaceAttribute,
                                    new XElement("Value", Constants.XsiNilAttribute));
            Console.WriteLine(elem);

            var foo = (FooWithIsNullable) serialier.Deserialize(elem.CreateReader());

            Assert.NotNull(foo.Value);
        }

        [Test]
        public void TestSerialize()
        {
            var foo = new FooWithIsNullable();

            var serialier = new XmlSerializer(typeof (FooWithIsNullable));

            var stream = new MemoryStream();
            serialier.Serialize(stream, foo);
            stream.Position = 0;

            var elem = XElement.Load(stream);

            Console.WriteLine(elem);
        }
    }
}