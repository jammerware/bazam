using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bazam.Tests
{
    [TestClass()]
    public class StringBeastTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void CapitalizeTest()
        {
            Assert.AreEqual("Sergei Temkin sucks.", StringBeast.Capitalize("sergei Temkin sucks."));
        }
        
        [TestMethod()]
        [TestCategory("Offline")]
        public void CompressionWorks()
        {
            string testString = "This compression crap totally works.";
            Assert.AreEqual(testString, StringBeast.Decompress(StringBeast.Compress(testString)));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StripNumbersTest()
        {
            Assert.AreEqual("123", StringBeast.StripIntegers("Sergei Temkin deserves 123 swirlies."));
        }
    }
}