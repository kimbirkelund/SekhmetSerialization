using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class VariousRootsTest
    {
        [Test]
        public void TestSerializingAList()
        {
            var serializer = new XmlSerializer(typeof(List<int>));

            var stream = new MemoryStream();

            serializer.Serialize(stream, new List<int> { 1, 2, 3 });

            stream.Position = 0;
            using (var reader = new StreamReader(stream))
                Console.WriteLine(reader.ReadToEnd());
        }

        [Test]
        public void TestSerializingAnArray()
        {
            var serializer = new XmlSerializer(typeof(int[]));

            var stream = new MemoryStream();

            serializer.Serialize(stream, new[] { 1, 2, 3 });

            stream.Position = 0;
            using (var reader = new StreamReader(stream))
                Console.WriteLine(reader.ReadToEnd());
        }

        [Test]
        public void TestSerializingAnArrayList()
        {
            var serializer = new XmlSerializer(typeof(ArrayList));

            var stream = new MemoryStream();

            serializer.Serialize(stream, new ArrayList { 1, 2, 3 });

            stream.Position = 0;
            using (var reader = new StreamReader(stream))
                Console.WriteLine(reader.ReadToEnd());
        }
    }
}