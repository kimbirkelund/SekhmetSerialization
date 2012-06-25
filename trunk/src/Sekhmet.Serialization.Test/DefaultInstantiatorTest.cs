using System;
using Xunit;

namespace Sekhmet.Serialization.Test
{
    public class DefaultInstantiatorTest
    {
        [Fact]
        public void TestCreate_Failure()
        {
            var instantiator = new DefaultInstantiator();

            Assert.Throws<MissingMethodException>(() => instantiator.Create(typeof(FooWithoutParameterlessConstructor)));
        }

        [Fact]
        public void TestCreate_Success()
        {
            var instantiator = new DefaultInstantiator();

            var actual = instantiator.Create(typeof(DefaultInstantiatorTest));

            Assert.NotNull(actual);
            Assert.IsType(typeof(DefaultInstantiatorTest), actual);
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