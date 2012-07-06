using System;
using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class BuildInCollectionsTypeConverterTest
    {
        [Test]
        public void TestGetActualType()
        {
            var converter = new BuiltInCollectionsTypeConverter();

            DoTest(converter, typeof(List<object>), typeof(IEnumerable));
            DoTest(converter, typeof(List<object>), typeof(ICollection));
            DoTest(converter, typeof(List<object>), typeof(IList));
            DoTest(converter, typeof(ArrayList), typeof(ArrayList));
            DoTest(converter, null, typeof(IDictionary));

            DoTest(converter, typeof(List<int>), typeof(IEnumerable<int>));
            DoTest(converter, typeof(List<int>), typeof(ICollection<int>));
            DoTest(converter, typeof(List<int>), typeof(IList<int>));
            DoTest(converter, typeof(List<int>), typeof(List<int>));
            DoTest(converter, null, typeof(IDictionary<int, string>));
        }

        private static void DoTest(BuiltInCollectionsTypeConverter converter, Type expectedType, Type input)
        {
            var actualType = converter.GetActualType(input, new Mock<IAdviceRequester>().Object);

            Assert.AreEqual(expectedType, actualType);
        }
    }
}