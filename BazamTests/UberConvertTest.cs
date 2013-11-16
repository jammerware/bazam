using System;
using System.Drawing;
using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BazamTests
{
    [TestClass()]
    public class UberConvertTest
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
        public void StringToBooleanThrowsExceptionWithBadArgument()
        {
            try {
                UberConvert.StringToBoolean(string.Empty);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToBooleanUpperCaseTest()
        {
            Assert.AreEqual(true, UberConvert.StringToBoolean("TRUE"));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToBooleanLowerCaseTest()
        {
            Assert.AreEqual(false, UberConvert.StringToBoolean("false"));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToColorWithNamedColor()
        {
            Assert.AreEqual(Color.Blue, UberConvert.StringToColor("blue"));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToColorWith3Hex()
        {
            Assert.AreEqual(Color.FromArgb(255, 255, 255), UberConvert.StringToColor("#FFF"));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToColorWith6Hex()
        {
            Assert.AreEqual(Color.FromArgb(255, 255, 255), UberConvert.StringToColor("#FFFFFF"));
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void StringToColorFailsWithNonsense()
        {
            try {
                UberConvert.StringToColor("Sergei Temkin is running for president... of Hell.");
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod()]
        [TestCategory("Offline")]
        public void HtmlToStringTest()
        {
            Assert.AreEqual("I want to hit Sergei Temkin" + Environment.NewLine + "with a carriage return.", UberConvert.HtmlToString("I want to hit Sergei Temkin<br />with a carriage return."));
        }
    }
}