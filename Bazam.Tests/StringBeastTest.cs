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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        [TestCategory("Offline")]
        public void CapitalizeTest()
        {
            Assert.AreEqual("Sergei Temkin sucks.", StringBeast.Capitalize("sergei Temkin sucks."));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StripNumbersTest()
        {
            Assert.AreEqual("123", StringBeast.StripIntegers("Sergei Temkin deserves 123 swirlies."));
        }
    }
}