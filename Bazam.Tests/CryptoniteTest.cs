using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bazam.Tests
{
    [TestClass()]
    public class CryptoniteTest
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
        [TestCategory("Online")]
        public void EncryptDecryptTest()
        {
            string input = "Sergei Temkin is a terrible human being.";
            string key = "m-U^VC]k5HoY(lr9NX@@,x,nD&G>_O^-";
            
            Assert.AreEqual(input, Cryptonite.Decrypt(Cryptonite.Encrypt(input, key), key));
        }
    }
}
