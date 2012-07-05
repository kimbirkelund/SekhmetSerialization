using System.Xml.Linq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Utility.Test
{
    [TestFixture]
    public class XObjectExtensionsTest
    {
        [Test]
        public void TestToFriendlyName_Attribute()
        {
            Assert.AreEqual("[@test=value]", new XAttribute("test", "value").ToFriendlyName());
        }

        [Test]
        public void TestToFriendlyName_Element()
        {
            var elem = new XElement("foo", new XElement("bar", new XAttribute("baz", "qut"), new XAttribute("foo", "bar")));

            string actual = elem.Element("bar").ToFriendlyName();

            Assert.AreEqual("[/foo/bar<@baz=qut, @foo=bar>]", actual);
        }
    }
}