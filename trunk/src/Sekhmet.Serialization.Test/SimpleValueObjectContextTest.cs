using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class SimpleValueObjectContextTest
    {
        [Test]
        public void TestConstructor_ErrorCases()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleValueObjectContext(null, 42));
            Assert.Throws<ArgumentNullException>(() => new SimpleValueObjectContext(typeof(int), null));
            Assert.Throws<ArgumentException>(() => new SimpleValueObjectContext(typeof(int), true));
        }

        [Test]
        public void TestProperties()
        {
            var context = new SimpleValueObjectContext<int>(42);

            Assert.AreEqual(typeof(int), context.Type);
            Assert.AreEqual(typeof(int), context.ContractType);
            Assert.IsFalse(context.Members.Any());
            Assert.IsFalse(context.Attributes.Any());

            Assert.AreEqual(42, context.GetObject());
        }
    }
}
