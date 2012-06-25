using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class SerializationManagerExtensionsTest
    {
        [Test]
        public void TestDeserializeFromLocation_StringGeneric()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            var sourceLocation = Path.GetTempFileName();
            expectedSource.Save(sourceLocation);
            manager.DeserializeFromLocation<SerializationManagerExtensionsTest>(sourceLocation);

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserializeFromLocation_StringNonGeneric()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            var sourceLocation = Path.GetTempFileName();
            expectedSource.Save(sourceLocation);
            manager.DeserializeFromLocation(sourceLocation, typeof(SerializationManagerExtensionsTest));

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserializeFromLocation_UriGeneric()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            var sourceLocation = new Uri("file:///" + Path.GetTempFileName());
            expectedSource.Save(sourceLocation.LocalPath);
            manager.DeserializeFromLocation<SerializationManagerExtensionsTest>(sourceLocation);

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserializeFromLocation_UriNonGeneric()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            var sourceLocation = new Uri("file:///" + Path.GetTempFileName());
            expectedSource.Save(sourceLocation.LocalPath);
            manager.DeserializeFromLocation(sourceLocation, typeof(SerializationManagerExtensionsTest));

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserializeFromString_Generic()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            manager.DeserializeFromString<SerializationManagerExtensionsTest>(expectedSource.ToString());

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserializeFromString_NonGeneric()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            manager.DeserializeFromString(expectedSource.ToString(), typeof(SerializationManagerExtensionsTest));

            Assert.True(XNode.DeepEquals(expectedSource, manager.DeserializeSource));
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestDeserialize_Generic()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new XElement("SMET");
            manager.Deserialize<SerializationManagerExtensionsTest>(expectedSource);

            Assert.AreEqual(expectedSource, manager.DeserializeSource);
            Assert.AreEqual(typeof(SerializationManagerExtensionsTest), manager.DeserializeTargetType);
            Assert.AreEqual(1, manager.DeserializeCallCount);
        }

        [Test]
        public void TestSerializeToString()
        {
            var manager = new SerializationManagerStub();

            var expectedSource = new object();
            manager.SerializeToString(expectedSource);

            Assert.AreEqual(expectedSource, manager.SerializeSource);
            Assert.AreEqual(1, manager.SerializeCallCount);
        }
    }
}