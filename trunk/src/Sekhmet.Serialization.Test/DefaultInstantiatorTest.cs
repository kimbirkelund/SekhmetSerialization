using System;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class DefaultInstantiatorTest
    {
        [Test]
        public void TestCreate_Failure()
        {
            var instantiator = new DefaultInstantiator();

            Assert.Throws<MissingMethodException>(() => instantiator.Create(typeof (FooWithoutParameterlessConstructor)));
        }

        [Test]
        public void TestCreate_Success()
        {
            var instantiator = new DefaultInstantiator();

            var actual = instantiator.Create(typeof (DefaultInstantiatorTest));

            Assert.NotNull(actual);
            Assert.IsInstanceOf(typeof (DefaultInstantiatorTest), actual);
        }

        private class FooWithoutParameterlessConstructor
        {
            public string Value { get; set; }

            public FooWithoutParameterlessConstructor(string value)
            {
                Value = value;
            }
        }
    }
}