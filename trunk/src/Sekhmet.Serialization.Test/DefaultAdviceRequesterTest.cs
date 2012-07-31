using System;
using System.Linq;
using NUnit.Framework;

namespace Sekhmet.Serialization.Test
{
    [TestFixture]
    public class DefaultAdviceRequesterTest
    {
        [Test]
        public void TestAddAdvisor()
        {
            var dar = new DefaultAdviceRequester(this);

            EventHandler<AdviceRequestedEventArgs> eventHandler1 = delegate { };
            EventHandler<AdviceRequestedEventArgs> eventHandler2 = delegate { };

            dar.AddAdvisor(eventHandler1, "Foo", "Bar");
            dar.AddAdvisor(eventHandler2, "Baz", "Bar");

            Assert.AreEqual(2, dar.Advisors.Count());
            DefaultAdviceRequester.AdvisorInfo ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();
            DefaultAdviceRequester.AdvisorInfo ai2 = dar.Advisors.Where(ai => ai.Advisor == eventHandler2).Single();
            CollectionAssert.AreEquivalent(new AdviceType[] { "Foo", "Bar" }, ai1.Types);
            CollectionAssert.AreEquivalent(new AdviceType[] { "Baz", "Bar" }, ai2.Types);

            dar.AddAdvisor(eventHandler1, CommonAdviceTypes.All);

            Assert.AreEqual(2, dar.Advisors.Count());
            ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();
            ai2 = dar.Advisors.Where(ai => ai.Advisor == eventHandler2).Single();
            CollectionAssert.AreEquivalent(new[] { CommonAdviceTypes.All }, ai1.Types);
            CollectionAssert.AreEquivalent(new AdviceType[] { "Baz", "Bar" }, ai2.Types);
        }

        [Test]
        public void TestRemoveAdvisor()
        {
            var dar = new DefaultAdviceRequester(this);

            EventHandler<AdviceRequestedEventArgs> eventHandler1 = delegate { };
            EventHandler<AdviceRequestedEventArgs> eventHandler2 = delegate { };

            dar.AddAdvisor(eventHandler1, CommonAdviceTypes.All);
            dar.AddAdvisor(eventHandler2, "Baz", "Bar");

            Assert.AreEqual(2, dar.Advisors.Count());

            DefaultAdviceRequester.AdvisorInfo ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();
            DefaultAdviceRequester.AdvisorInfo ai2 = dar.Advisors.Where(ai => ai.Advisor == eventHandler2).Single();

            CollectionAssert.AreEquivalent(new[] { CommonAdviceTypes.All }, ai1.Types);
            CollectionAssert.AreEquivalent(new AdviceType[] { "Baz", "Bar" }, ai2.Types);

            dar.RemoveAdvisor(eventHandler2, "Baz");

            Assert.AreEqual(2, dar.Advisors.Count());
            ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();
            ai2 = dar.Advisors.Where(ai => ai.Advisor == eventHandler2).Single();

            CollectionAssert.AreEquivalent(new[] { CommonAdviceTypes.All }, ai1.Types);
            CollectionAssert.AreEquivalent(new AdviceType[] { "Bar" }, ai2.Types);

            dar.RemoveAdvisor(eventHandler2, "Bar");

            Assert.AreEqual(1, dar.Advisors.Count());
            ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();

            CollectionAssert.AreEquivalent(new[] { CommonAdviceTypes.All }, ai1.Types);

            dar.RemoveAdvisor(eventHandler1, "Bar");

            Assert.AreEqual(1, dar.Advisors.Count());
            ai1 = dar.Advisors.Where(ai => ai.Advisor == eventHandler1).Single();

            CollectionAssert.AreEquivalent(new[] { CommonAdviceTypes.All }, ai1.Types);

            dar.RemoveAdvisor(eventHandler1, CommonAdviceTypes.All);

            Assert.AreEqual(0, dar.Advisors.Count());
        }

        [Test]
        public void TestRequestAdvice()
        {
            var dar = new DefaultAdviceRequester(this);

            int foo1 = 0;
            int bar1 = 0;
            int foo2 = 0;
            int baz2 = 0;
            int foo3 = 0;
            int bar3 = 0;
            int baz3 = 0;
            int qut3 = 0;

            EventHandler<AdviceRequestedEventArgs> advisor1 = (s, e) =>
            {
                Assert.AreSame(this, s);
                switch (e.Type)
                {
                    case "Foo":
                        foo1++;
                        break;
                    case "Bar":
                        bar1++;
                        break;
                    default:
                        Assert.Fail("Unexpected type: " + e.Type);
                        break;
                }
            };
            EventHandler<AdviceRequestedEventArgs> advisor2 = (s, e) =>
            {
                Assert.AreSame(this, s);
                switch (e.Type)
                {
                    case "Foo":
                        foo2++;
                        break;
                    case "Baz":
                        baz2++;
                        break;
                    default:
                        Assert.Fail("Unexpected type: " + e.Type);
                        break;
                }
            };
            EventHandler<AdviceRequestedEventArgs> advisor3 = (s, e) =>
            {
                Assert.AreSame(this, s);
                switch (e.Type)
                {
                    case "Foo":
                        foo3++;
                        break;
                    case "Bar":
                        bar3++;
                        break;
                    case "Baz":
                        baz3++;
                        break;
                    case "Qut":
                        qut3++;
                        break;
                    default:
                        Assert.Fail("Unexpected type: " + e.Type);
                        break;
                }
            };

            dar.AddAdvisor(advisor1, "Foo", "Bar");
            dar.AddAdvisor(advisor2, "Foo", "Baz");
            dar.AddAdvisor(advisor3, CommonAdviceTypes.All);

            dar.RequestAdvice(new AdviceRequestedEventArgs("Foo"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Bar"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Baz"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Qut"));

            Assert.AreEqual(1, foo1);
            Assert.AreEqual(1, foo2);
            Assert.AreEqual(1, foo3);
            Assert.AreEqual(1, bar1);
            Assert.AreEqual(1, bar3);
            Assert.AreEqual(1, baz2);
            Assert.AreEqual(1, baz3);
            Assert.AreEqual(1, qut3);

            dar.RequestAdvice(new AdviceRequestedEventArgs("Foo"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Bar"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Baz"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Qut"));

            Assert.AreEqual(2, foo1);
            Assert.AreEqual(2, foo2);
            Assert.AreEqual(2, foo3);
            Assert.AreEqual(2, bar1);
            Assert.AreEqual(2, bar3);
            Assert.AreEqual(2, baz2);
            Assert.AreEqual(2, baz3);
            Assert.AreEqual(2, qut3);

            dar.RequestAdvice(new AdviceRequestedEventArgs("Bar"));
            dar.RequestAdvice(new AdviceRequestedEventArgs("Qut"));

            Assert.AreEqual(2, foo1);
            Assert.AreEqual(2, foo2);
            Assert.AreEqual(2, foo3);
            Assert.AreEqual(3, bar1);
            Assert.AreEqual(3, bar3);
            Assert.AreEqual(2, baz2);
            Assert.AreEqual(2, baz3);
            Assert.AreEqual(3, qut3);
        }
    }
}