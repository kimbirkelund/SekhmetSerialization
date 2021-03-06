﻿using System;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class ActualTypeFromAttributeTypeConverterTest
    {
        [Test]
        public void GetActualType_NonXElementArgument()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Null(converter.GetActualType(new XAttribute("type", "bob"), null, new Mock<IAdviceRequester>().Object));
        }

        [Test]
        public void GetActualType_NullArguments()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Throws<ArgumentNullException>(() => converter.GetActualType(null, null, new Mock<IAdviceRequester>().Object));
        }

        [Test]
        public void GetActual_ElementArgumentWithTypeAttribute()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            var input = new XElement("Elem", new XAttribute("type", typeof (ActualTypeFromAttributeTypeConverterTest).AssemblyQualifiedName));

            Assert.AreEqual(typeof(ActualTypeFromAttributeTypeConverterTest), converter.GetActualType(input, null, new Mock<IAdviceRequester>().Object));
        }

        [Test]
        public void GetActual_ElementArgumentWithoutTypeAttribute()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Null(converter.GetActualType(new XElement("Elem"), null, new Mock<IAdviceRequester>().Object));
        }
    }
}