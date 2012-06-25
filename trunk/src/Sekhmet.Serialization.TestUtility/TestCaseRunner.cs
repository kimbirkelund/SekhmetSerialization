using System;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility;
using Xunit.Extensions;

namespace Sekhmet.Serialization.TestUtility
{
    public abstract class TestCaseRunnerTestBase
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

        private static IEnumerable<object[]> GetTestCases()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubTypeOf<ISerializationTestCase>())
                .Where(t => t.IsClass)
                .Where(t => !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ISerializationTestCase>()
                .Select(tc => new[] {tc})
                .ToList();
        }
    }
}