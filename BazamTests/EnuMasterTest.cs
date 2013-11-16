using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BazamTests
{
    [TestClass()]
    public class EnuMasterTest
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

        #region Helpers
        private enum TestEnum
        {
            YES,
            NO,
            MAYBE
        }

        private struct TestStruct
        {
        }
        #endregion

        [TestMethod()]
        [TestCategory("Offline")]
        public void ParseSuccessfulTest()
        {
            TestEnum result = EnuMaster.Parse<TestEnum>("maybe");
            Assert.AreEqual(TestEnum.MAYBE, result);
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void ParseFailsWithExceptionTest()
        {
            try {
                EnuMaster.Parse<TestEnum>("BABIES!");
                Assert.Fail();
            }
            catch (Exception) {}
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void ParseFailsButDoesntThrowTest()
        {
            TestEnum result = EnuMaster.Parse<TestEnum>("BABIES!", true, false);
            Assert.AreEqual(default(TestEnum), result);
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void ParseCantUseNonEnumsTest()
        {
            try {
                TestStruct result = EnuMaster.Parse<TestStruct>("maybe");
                Assert.Fail();
            }
            catch (InvalidOperationException) {
            }
            catch (Exception) {
                Assert.Fail();
            }
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void ParseCaseSensitive()
        {
            try {
                TestEnum result = EnuMaster.Parse<TestEnum>("maybe", false);
                Assert.Fail();
            }
            catch { }
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void TryParseSuccessTest()
        {
            TestEnum test = default(TestEnum);
            bool result = EnuMaster.TryParse<TestEnum>("MAYBE", out test);
            Assert.AreEqual(TestEnum.MAYBE, test);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void TryParseFailureTest()
        {
            TestEnum test = default(TestEnum);
            bool result = EnuMaster.TryParse<TestEnum>("BABIES!", out test);
            Assert.AreEqual(false, result);
            Assert.AreEqual(default(TestEnum), test);
        }
    }
}
