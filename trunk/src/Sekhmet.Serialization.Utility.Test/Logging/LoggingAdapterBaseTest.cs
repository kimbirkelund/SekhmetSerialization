using System;
using NUnit.Framework;
using Sekhmet.Serialization.Utility.Logging;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    [TestFixture]
    public class LoggingAdapterBaseTest
    {
        public static volatile int ConstructorCalls = 0;
        public static volatile int DestructorCalls = 0;

        [SetUp]
        public void SetupTest()
        {
            ConstructorCalls = 0;
            DestructorCalls = 0;
        }

        [Test]
        public void TestGetLogger_GCed()
        {
            var adapter = new LoggingAdapterStub();
            adapter.SetLevel("Foo.Bar.Baz", LogLevel.Info);

            GetAndAssertNotNull(adapter);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(1, ConstructorCalls);
            Assert.AreEqual(1, DestructorCalls);

            GetAndAssertNotNull(adapter);
            Assert.AreEqual(2, ConstructorCalls);
        }

        [Test]
        public void TestSimpleInstantiation_GCed()
        {
            CreateAndAssertNotNull();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(1, ConstructorCalls);
            Assert.AreEqual(1, DestructorCalls);

            CreateAndAssertNotNull();
            Assert.AreEqual(2, ConstructorCalls);
        }

        private static void CreateAndAssertNotNull()
        {
            Assert.NotNull(new LoggerStub());
        }

        private static void GetAndAssertNotNull(LoggingAdapterStub adapter)
        {
            Assert.NotNull(adapter.GetLogger("Foo.Bar.Baz"));
        }
    }
}