using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sekhmet.Serialization.Utility.Logging;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class DefaultConsoleWriterTest
    {
        public IEnumerable<object[]> Input
        {
            get { return GetTestCases().ToList(); }
        }

        [TestCaseSource("Input")]
        public void TestWriteLine(ConsoleColor color, string message, Exception ex)
        {
            var writer = new DefaultConsoleWriter();

            writer.WriteLine(color, message, ex);
        }

        private IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] { ConsoleColor.Red, "Bob", null };
            yield return new object[] { ConsoleColor.Red, "Bob", new Exception("error") };
        }
    }
}