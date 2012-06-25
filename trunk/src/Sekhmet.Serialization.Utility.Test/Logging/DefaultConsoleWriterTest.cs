using System;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility.Logging;
using Xunit.Extensions;

namespace Sekhmet.Serialization.Utility.Test.Logging
{
    public class DefaultConsoleWriterTest
    {
        public static IEnumerable<object[]> Input
        {
            get { return GetTestCases().ToList(); }
        }

        [Theory]
        [PropertyData("Input")]
        public void TestWriteLine(ConsoleColor color, string message, Exception ex)
        {
            var writer = new DefaultConsoleWriter();

            writer.WriteLine(color, message, ex);
        }

        private static IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] {ConsoleColor.Red, "Bob", null};
            yield return new object[] {ConsoleColor.Red, "Bob", new Exception("error")};
        }
    }
}