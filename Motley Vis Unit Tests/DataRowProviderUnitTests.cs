using System;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Motley_Vis;

namespace Motley_Vis_Unit_Tests
{
    [TestClass]
    public class DataRowProviderUnitTests
    {
        [TestMethod]
        public void CommaDelimitedTest()
        {
            var newProvider = new DataRowProvider(@"E:\Test_Data\small_csv\people_5_col_with_headers.csv", new char[] {','});

            Assert.AreEqual(4, newProvider.Count);
            Assert.AreEqual(5, newProvider[0].Count);
            Assert.AreEqual(newProvider.Count, newProvider.Headers.Count);
            Assert.AreEqual("First", newProvider.Headers[0]);
            Assert.AreEqual("Favorite Color", newProvider.Headers[4]);
        }
    }
}
