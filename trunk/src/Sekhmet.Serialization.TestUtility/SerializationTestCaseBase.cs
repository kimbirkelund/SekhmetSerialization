using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework;

namespace Sekhmet.Serialization.TestUtility
{
    public abstract class SerializationTestCaseBase : ISerializationTestCase
    {
        private object _object;
        private ISerializationManagerFactory _serializationManager;
        private XElement _xml;

        public virtual object Object
        {
            get { return _object ?? (_object = CreateObject()); }
        }

        public virtual ISerializationManagerFactory SerializationManager
        {
            get { return _serializationManager ?? (_serializationManager = CreateSerializationManager()); }
        }

        public virtual XElement Xml
        {
            get { return _xml ?? (_xml = CreateXml()); }
        }

        public virtual void AssertCorrectObject(object actual)
        {
            AssertCorrectObject(Object, actual);
        }

        public virtual void AssertCorrectXml(XElement actual)
        {
            AssertCorrectXml(Xml, actual);
        }

        protected virtual void AssertCorrectObject(object expected, object actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.AreEqual(expected.GetType(), actual.GetType());

            AssertDeepEquals(expected, actual);
        }

        protected virtual void AssertCorrectXml(XElement expected, XElement actual)
        {
            Console.WriteLine("===");
            Console.WriteLine("Expected:\n" + expected);
            Console.WriteLine("===");
            Console.WriteLine("Actual:\n" + actual);
            Console.WriteLine("===");

            Assert.NotNull(expected);
            Assert.NotNull(actual);

            AssertDeepEquals(expected, actual);
        }

        protected virtual void AssertDeepEquals(XElement expected, XElement actual)
        {
            if (expected == null)
            {
                Assert.Null(actual);
                return;
            }

            Assert.AreEqual(expected.Name, actual.Name);

            AssertDeepEquals(expected.Attributes(), actual.Attributes());

            if (expected.Elements().Any())
                AssertDeepEquals(expected.Elements(), actual.Elements());
            else
                Assert.AreEqual(expected.Value, actual.Value);
        }

        protected abstract object CreateObject();
        protected abstract ISerializationManagerFactory CreateSerializationManager();
        protected abstract XElement CreateXml();

        private void AssertDeepEquals(IEnumerable<XElement> expected, IEnumerable<XElement> actual)
        {
            expected = expected.ToList();
            actual = actual.ToList();

            Assert.AreEqual(expected.Count(), actual.Count());

            foreach (var pair in expected.Zip(actual, (f, s) => new { f, s }))
                AssertDeepEquals(pair.f, pair.s);
        }

        private static void AssertDeepEquals(IEnumerable<XAttribute> expected, IEnumerable<XAttribute> actual)
        {
            expected = expected.ToList();
            actual = actual.ToList();

            Assert.AreEqual(expected.Count(), actual.Count());

            foreach (var pair in expected.Zip(actual, (f, s) => new { f, s }))
            {
                Assert.AreEqual(pair.f.Name, pair.s.Name);
                Assert.AreEqual(pair.f.Value, pair.s.Value);
            }
        }

        private static void AssertDeepEquals(object expected, object actual)
        {
            // ReSharper disable PossibleNullReferenceException
            if (expected == null)
            {
                Assert.Null(actual);
                return;
            }

            Assert.IsInstanceOf(expected.GetType(), actual);

            if (expected.GetType().IsValueType)
                Assert.AreEqual(expected, actual);
            else if (expected is string)
                Assert.AreEqual(expected, actual);
            else if (expected is IEnumerable)
            {
                foreach (var pair in ((IEnumerable)expected).Cast<object>().Zip(((IEnumerable)actual).Cast<object>(), (f, s) => new { f, s }))
                    AssertDeepEquals(pair.f, pair.s);
            }
            else
            {
                foreach (var property in expected.GetType().GetProperties())
                {
                    var expectedPropValue = GetExpectedPropertyValue(expected, property);
                    var actualPropValue = GetActualPropertyValue(actual, property);
                    AssertDeepEquals(expectedPropValue, actualPropValue);
                }
            }
            // ReSharper restore PossibleNullReferenceException
        }

        private static object GetActualPropertyValue(object actual, PropertyInfo property)
        {
            try
            {
                return property.GetValue(actual, null);
            }
            catch (Exception ex)
            {
                Assert.True(false, "Cannot read actual value for '" + property + "': " + ex);
                throw;
            }
        }

        private static object GetExpectedPropertyValue(object expected, PropertyInfo property)
        {
            try
            {
                return property.GetValue(expected, null);
            }
            catch (Exception ex)
            {
                Assert.True(false, "Cannot read expected value for '" + property + "': " + ex);
                throw;
            }
        }
    }
}