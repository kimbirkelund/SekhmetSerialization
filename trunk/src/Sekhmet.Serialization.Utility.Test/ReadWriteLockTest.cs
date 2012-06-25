using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Sekhmet.Serialization.Utility.Test
{
    [TestFixture]
    public class ReadWriteLockTest
    {
        [Test]
        public void TestReadWriteLock_MultipleWrites()
        {
            var rwlock = new ReadWriteLock();
            var random = new Random();
            int value = 0;

            Action action = () =>
            {
                using (rwlock.EnterWriteScope())
                {
                    var v = value;
                    Thread.Sleep(random.Next(10));
                    var tmp = v + 100;
                    Thread.Sleep(random.Next(10));
                    value = tmp;
                }
            };

            var tasks = Enumerable.Range(1, 100)
                .Select(i => Task.Factory.StartNew(action))
                .ToArray();

            Task.WaitAll(tasks);

            const int expected = 100 * 100;

            Assert.AreEqual(expected, value);
        }

        [Test]
        public void TestReadWriteLock_NoWriteWhileReading()
        {
            // ReSharper disable AccessToModifiedClosure
            // ReSharper disable AccessToDisposedClosure
            // ReSharper disable ImplicitlyCapturedClosure
            using (var rwlock = new ReadWriteLock())
            {
                string value = "foo";

                const string expected = "foo";
                string actual = null;

                var ewh1 = new EventWaitHandle(false, EventResetMode.ManualReset);
                var ewh2 = new EventWaitHandle(false, EventResetMode.ManualReset);

                var task1 = Task.Factory.StartNew(() =>
                {
                    using (rwlock.EnterReadScope())
                    {
                        ewh1.Set();
                        Assert.False(ewh2.WaitOne(TimeSpan.FromMilliseconds(100)));
                        Thread.Sleep(100);

                        actual = value;
                    }
                    Assert.True(ewh2.WaitOne(TimeSpan.FromSeconds(5)));
                });

                var task2 = Task.Factory.StartNew(() =>
                {
                    Assert.True(ewh1.WaitOne(TimeSpan.FromSeconds(5)));
                    using (rwlock.EnterWriteScope())
                        value = "bar";
                    ewh2.Set();
                });

                Assert.True(task1.Wait(TimeSpan.FromSeconds(5)));
                Assert.True(task2.Wait(TimeSpan.FromSeconds(5)));

                Assert.AreEqual(expected, actual);
            }
            // ReSharper restore ImplicitlyCapturedClosure
            // ReSharper restore AccessToDisposedClosure
            // ReSharper restore AccessToModifiedClosure
        }

        [Test]
        public void TestReadWriteLock_NonConcurrent()
        {
            using (var rwlock = new ReadWriteLock())
            {
                using (rwlock.EnterReadScope()) { }
                using (rwlock.EnterWriteScope()) { }
                using (rwlock.EnterUpgradeableReadScope())
                using (rwlock.EnterWriteScope()) { }

                rwlock.Dispose();
            }
        }
    }
}