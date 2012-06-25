using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization.TestUtility
{
    public abstract class TestCaseRunnerTestBase
    {
        public IEnumerable<object[]> TestSerializationTestCasesData
        {
            get { return GetTestCases(); }
        }

        [TestCaseSource("TestSerializationTestCasesData")]
        public void TestDeserializationTestCases(ISerializationTestCase testCase)
        {
            var manager = testCase.SerializationManager.Create();

            var actual = manager.Deserialize(testCase.Xml, testCase.Object.GetType());

            testCase.AssertCorrectObject(actual);
        }

        [TestCaseSource("TestSerializationTestCasesData")]
        public void TestSerializationTestCases(ISerializationTestCase testCase)
        {
            var manager = testCase.SerializationManager.Create();

            var actual = manager.Serialize(testCase.Object);

            testCase.AssertCorrectXml(actual);
        }

        private IEnumerable<object[]> GetTestCases()
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