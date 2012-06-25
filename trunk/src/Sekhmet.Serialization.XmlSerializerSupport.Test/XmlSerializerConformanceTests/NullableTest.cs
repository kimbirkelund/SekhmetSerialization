using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class NullableTest
    {
        [Test]
        public void TestSerialize()
        {
            var foo = new Foo {
                                  Bar1 = new Bar()
                              };

            var serialier = new XmlSerializer(typeof (Foo));

            var stream = new MemoryStream();
            serialier.Serialize(stream, foo);
            stream.Position = 0;

            var elem = XElement.Load(stream);

            Console.WriteLine(elem);
        }
    }
}