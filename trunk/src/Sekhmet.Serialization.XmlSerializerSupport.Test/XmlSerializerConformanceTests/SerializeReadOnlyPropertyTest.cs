using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class SerializeReadOnlyPropertyTest
    {
        [Test]
        public void TestMethod1()
        {
            var xmlSerializer = new XmlSerializer(typeof(FooWithReadOnlyProperty));

            var stream = new MemoryStream();

            xmlSerializer.Serialize(stream, new FooWithReadOnlyProperty());

            stream.Position = 0;
            using (var reader = new StreamReader(stream))
                Console.WriteLine(reader.ReadToEnd());
        }
    }
}