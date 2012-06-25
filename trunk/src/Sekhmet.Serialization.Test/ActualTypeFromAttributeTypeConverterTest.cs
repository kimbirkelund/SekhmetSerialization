using System;
using System.Xml.Linq;
using Xunit;

namespace Sekhmet.Serialization.Test
{
    public class ActualTypeFromAttributeTypeConverterTest
    {
        [Fact]
        public void GetActualType_NonXElementArgument()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Null(converter.GetActualType(new XAttribute("type", "bob"), null));
        }

        [Fact]
        public void GetActualType_NullArguments()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Throws<ArgumentNullException>(() => converter.GetActualType(null, null));
        }

        [Fact]
        public void GetActual_ElementArgumentWithTypeAttribute()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            var input = new XElement("Elem", new XAttribute("type", typeof (ActualTypeFromAttributeTypeConverterTest).AssemblyQualifiedName));

            Assert.Equal(typeof (ActualTypeFromAttributeTypeConverterTest), converter.GetActualType(input, null));
        }

        [Fact]
        public void GetActual_ElementArgumentWithoutTypeAttribute()
        {
            var converter = new ActualTypeFromAttributeTypeConverter();

            Assert.Null(converter.GetActualType(new XElement("Elem"), null));
        }
    }
}