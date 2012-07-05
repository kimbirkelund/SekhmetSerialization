using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    public class ValueTypeDeserializerHelperTest
    {
        public IEnumerable<object> TestDeserializeFromValueData
        {
            get { return GetTestDeserializeFromValueData().ToList(); }
        }

        [TestCaseSource("TestDeserializeFromValueData")]
        public void TestDeserializeFromValue(object expectedValue)
        {
            var helper = new ValueTypeDeserializerHelper();

            string value = new XElement("value", expectedValue).Value;
            Type type = expectedValue.GetType();

            IObjectContext actual = helper.DeserializeFromValue(value, type);

            Assert.IsFalse(actual.Attributes.Any());
            Assert.IsFalse(actual.Members.Any());

            Assert.AreEqual(expectedValue, actual.GetObject());
            Assert.AreEqual(type, actual.Type);
        }

        private IEnumerable<object> GetTestDeserializeFromValueData()
        {
            yield return true;
            yield return false;
            yield return 'a';
            yield return '^';
            yield return sbyte.MinValue;
            yield return sbyte.MaxValue;
            yield return byte.MinValue;
            yield return byte.MaxValue;
            yield return short.MinValue;
            yield return short.MaxValue;
            yield return ushort.MinValue;
            yield return ushort.MaxValue;
            yield return int.MinValue;
            yield return int.MaxValue;
            yield return uint.MinValue;
            yield return uint.MaxValue;
            yield return long.MinValue;
            yield return long.MaxValue;
            yield return ulong.MinValue;
            yield return ulong.MaxValue;
            yield return float.MinValue;
            yield return float.MaxValue;
            yield return double.MinValue;
            yield return double.MaxValue;
            yield return decimal.MinValue;
            yield return decimal.MaxValue;
            yield return DateTime.MinValue;
            yield return new DateTime(2012, 7, 5, 13, 57, 38);
            yield return DateTime.MaxValue;
        }
    }
}