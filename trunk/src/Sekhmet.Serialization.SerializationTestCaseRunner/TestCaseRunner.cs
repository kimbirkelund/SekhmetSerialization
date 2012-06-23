using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sekhmet.Serialization.TestUtility;
using Sekhmet.Serialization.Utility;
using Xunit;
using Xunit.Extensions;

namespace Sekhmet.Serialization.SerializationTestCaseRunner
{
    public class SerializationTest
    {
        public static IEnumerable<object[]> TestSerializationTestCasesData
        {
            get { return GetTestCases(); }
        }

        [Theory]
        [PropertyData("TestSerializationTestCasesData")]
        public void TestDeserializationTestCases(ISerializationTestCase testCase)
        {
            var manager = testCase.SerializationManager.Create();

            var actual = manager.Deserialize(testCase.Xml, testCase.Object.GetType());

            testCase.AssertCorrectObject(actual);
        }

        [Theory]
        [PropertyData("TestSerializationTestCasesData")]
        public void TestSerializationTestCases(ISerializationTestCase testCase)
        {
            var manager = testCase.SerializationManager.Create();

            var actual = manager.Serialize(testCase.Object);

            testCase.AssertCorrectXml(actual);
        }

        [Fact]
        public void TestToEnsureThatTheTheoriesAreRun() { }

        private static IEnumerable<object[]> GetTestCases()
        {
            return Directory.GetFiles(Environment.CurrentDirectory, "*.dll")
                    .Select(Assembly.LoadFile)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsSubTypeOf<ISerializationTestCase>())
                    .Where(t => t.IsClass)
                    .Where(t => !t.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<ISerializationTestCase>()
                    .Select(tc => new[] { tc })
                    .ToList();
        }
    }
}