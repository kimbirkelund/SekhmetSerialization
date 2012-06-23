using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;
using Xunit;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    public class SerializeNullValueTest
    {
        [Fact]
        public void TestSerializeNullValues()
        {
            var foo = new Foo {
                                  Bar1 = new Bar()
                              };

            var xmlSerialier = new XmlSerializer(typeof (Foo));

            var stream = new MemoryStream();

            xmlSerialier.Serialize(stream, foo);
            stream.Position = 0;

            var elem = XElement.Load(stream);

            Console.WriteLine(elem);
        }
    }
}