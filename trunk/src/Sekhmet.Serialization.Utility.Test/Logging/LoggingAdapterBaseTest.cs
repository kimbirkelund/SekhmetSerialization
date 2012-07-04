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
        
        [Test]
        public void TestGetLogger_GCed()
        {
            ConstructorCalls = 0;
            DestructorCalls = 0;

            var adapter = new LoggingAdapterStub();
            adapter.SetLevel("Foo.Bar.Baz", LogLevel.Info);

            GetAndAssertNotNull(adapter);
            Assert.AreEqual(1, ConstructorCalls, "Expected ConstructorCalls == 1");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(1, DestructorCalls, "Expected DestructorCalls == 1");

            GetAndAssertNotNull(adapter);

            Assert.AreEqual(2, ConstructorCalls, "Expected ConstructorCalls == 2");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        
            Assert.AreEqual(2, DestructorCalls, "Expected DestructorCalls == 2");
        }

        [Test]
        public void TestSimpleInstantiation_GCed()
        {
            ConstructorCalls = 0;
            DestructorCalls = 0;
            
            CreateAndAssertNotNull();
            Assert.AreEqual(1, ConstructorCalls, "Expected ConstructorCalls == 1");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(1, DestructorCalls, "Expected DestructorCalls == 1");

            CreateAndAssertNotNull();

            Assert.AreEqual(2, ConstructorCalls, "Expected ConstructorCalls == 2");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(2, DestructorCalls, "Expected DestructorCalls == 2");
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