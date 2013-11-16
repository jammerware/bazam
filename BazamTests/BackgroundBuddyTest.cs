using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bazam.DataNinja;
using System.Data;
using BazamTests.Core;

namespace BazamTests
{
    [TestClass()]
    public class BackgroundBuddyTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataNinja myNinja = new DataNinja(
                Desmond.TEST_SERVER,
                Desmond.TEST_DB,
                Desmond.TEST_USERNAME,
                Desmond.TEST_PASSWORD
            );

            myNinja.NonQuery(new DataNinjaQuery("TRUNCATE TABLE Test_BackgroundBuddy", CommandType.Text));
        }
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
        public void RunAsyncTest()
        {
            DataNinja myNinja = new DataNinja(
                Desmond.TEST_SERVER,
                Desmond.TEST_DB,
                Desmond.TEST_USERNAME,
                Desmond.TEST_PASSWORD
            );
            
            BackgroundBuddy.RunAsync(() => { myNinja.NonQuery(new DataNinjaQuery("INSERT INTO Test_BackgroundBuddy(Value) SELECT 'lolwtf'; ", CommandType.Text)); });
            object tableHasValues = myNinja.GetScalar(new DataNinjaQuery("SELECT CASE WHEN EXISTS(SELECT * FROM Test_BackgroundBuddy) THEN 1 ELSE 0 END;", CommandType.Text));

            Assert.AreEqual(0, tableHasValues);
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void RunAsyncWithCallbackTest()
        {
            DataNinja myNinja = new DataNinja(
                Desmond.TEST_SERVER,
                Desmond.TEST_DB,
                Desmond.TEST_USERNAME,
                Desmond.TEST_PASSWORD
            );

            object tableHasValues = null;
            BackgroundBuddy.RunAsync(
                () => { myNinja.NonQuery(new DataNinjaQuery("INSERT INTO Test_BackgroundBuddy(Value) SELECT 'lolwtf'; ", CommandType.Text)); },
                () => { 
                    tableHasValues = myNinja.GetScalar(new DataNinjaQuery("SELECT CASE WHEN EXISTS(SELECT * FROM Test_BackgroundBuddy) THEN 1 ELSE 0 END;", CommandType.Text));
                    Assert.AreEqual(1, tableHasValues);
                }
            );
        }
    }
}
