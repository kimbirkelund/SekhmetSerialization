using System;
using System.IO;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;
using Xunit;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    public class SerializeReadOnlyPropertyTest
    {
        [Fact]
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