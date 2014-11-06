using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Motley_Vis;

namespace Motley_Vis_Unit_Tests
{
    [TestClass]
    public class LruCacheUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void NegativeCapacityTest()
        {
            var newCache = new LruCache<int, double>(-1);
        }

        [TestMethod]
        public void StillContainsCapacityTest_Success()
        {
            var newCache = new LruCache<int, long>(100);

            for (int i = 1; i <= 100; i++)
            {
                newCache.Add(i, i*i);
            }

            Assert.AreEqual(1, newCache.TryGet(1, -1));
            Assert.AreEqual(100, newCache.TryGet(10, -1));
            Assert.AreEqual(10000, newCache.TryGet(100, -1));
        }

        [TestMethod]
        public void StillContainsCapacityTest_Fail()
        {
            var newCache = new LruCache<int, long>(100);

            for (int i = 0; i < 1000; i++)
            {
                newCache.Add(i, i * i);
            }

            Assert.AreEqual(-1, newCache.TryGet(1, -1));
        }
    }
}
