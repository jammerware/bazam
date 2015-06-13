using Bazam.DataNinja;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Diagnostics;
using Bazam.Modules;
using BazamTests.Core;

namespace Bazam.Tests
{
    [TestClass()]
    public class DataNinjaTest
    {
        private TestContext testContextInstance;
        private static DataNinjaClient myNinja;
        private static DataNinjaClient myWindowsAuthenticationNinja;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            myNinja = new DataNinjaClient(Desmond.TEST_SERVER, Desmond.TEST_DB, Desmond.TEST_USERNAME, Desmond.TEST_PASSWORD);
            myWindowsAuthenticationNinja = new DataNinjaClient(Desmond.TEST_SERVER, Desmond.TEST_DB);
        }

        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            myNinja.NonQuery(
                new DataNinjaQuery(
                    "TRUNCATE TABLE Test_Varchar; TRUNCATE TABLE Test_KeyValues;",
                    CommandType.Text
                )
            );
        }

        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{ 
        //}
        #endregion

        #region Internal Helpers
        private void CallbackHelper(IAsyncResult result)
        {
            Debug.WriteLine("CALLED THAT CRAP BACK: " + result.ToString());
        }
        #endregion

        #region Tests
        [TestMethod()]
        [TestCategory("Online")]
        public void WindowsAuthenticationFailureTest()
        {
            try {
                myWindowsAuthenticationNinja.NonQuery(new DataNinjaQuery("UPDATE Bank_Employees SET EmployeeUserID = 'bstein' WHERE EmployeeID=484", CommandType.Text));
                Assert.Fail();
            }
            catch (DataNinjaException) {
            }
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void NonQueryAndGetScalarTest()
        {
            string value = "Plaintext insert works.";
            DataNinjaQuery query = new DataNinjaQuery(
                "INSERT INTO Test_Varchar(VarcharValue) VALUES(@InsertText);",
                CommandType.Text,
                new SqlParameter("@InsertText", value)
            );
            myNinja.NonQuery(query);

            Assert.AreEqual(value, myNinja.GetScalar(new DataNinjaQuery("SELECT TOP 1 VarcharValue FROM Test_Varchar;", CommandType.Text)));
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void NonQueryAsyncTest()
        {
            string value = "Async insert works.";
            DataNinjaQuery query = new DataNinjaQuery(
                "INSERT INTO Test_Varchar(VarcharValue) VALUES(@InsertText);",
                CommandType.Text,
                new SqlParameter("@InsertText", value)
            );

            myNinja.NonQueryAsync(query);            
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void NonQueryAsyncTestCallback()
        {
            string value = "Async insert with callback works.";
            DataNinjaQuery query = new DataNinjaQuery(
                "INSERT INTO Test_Varchar(VarcharValue) VALUES(@InsertText);",
                CommandType.Text,
                new SqlParameter("@InsertText", value)
            );

            myNinja.NonQueryAsync(query, new AsyncCallback(CallbackHelper));
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void NonQueryTransactionTest()
        {
            myNinja.NonQueryTransaction(
                new DataNinjaQuery(
                    "INSERT INTO Test_KeyValues(ItemID, ItemDesc) VALUES(1, 'Yea');",
                    CommandType.Text
                ),
                new DataNinjaQuery(
                    "INSERT INTO Test_KeyValues(ItemID, ItemDesc) VALUES(2, 'Nay');",
                    CommandType.Text
                )
            );

            Assert.AreEqual("Yea", myNinja.GetScalar(new DataNinjaQuery("SELECT ItemDesc FROM Test_KeyValues WHERE ItemID=1", CommandType.Text)));
            Assert.AreEqual("Nay", myNinja.GetScalar(new DataNinjaQuery("SELECT ItemDesc FROM Test_KeyValues WHERE ItemID=2", CommandType.Text)));
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void NonQueryTransactionFailureTest()
        {
            DataNinjaQuery goodStatement = new DataNinjaQuery("INSERT INTO Test_KeyValues(ItemID, ItemDesc) VALUES(1, 'Legit statement')", CommandType.Text);
            DataNinjaQuery badStatement = new DataNinjaQuery("INSERT INTO FarkleTheNonTable(ItemID, ItemDesc) VALUES(2, 'Bogus statement')", CommandType.Text);
            try {
                myNinja.NonQueryTransaction(
                    goodStatement,
                    badStatement
                );
            }
            catch (DataNinjaTransactionException transEx) {
                Assert.AreSame(transEx.Query, badStatement);
            }
            finally {
                Assert.IsFalse(
                    UberConvert.StringToBoolean(
                        myNinja.GetScalar(
                            new DataNinjaQuery(
                                "SELECT CASE WHEN EXISTS(SELECT * FROM Test_KeyValues WHERE ItemID = 1) THEN 'true' ELSE 'false' END;",
                                CommandType.Text
                            )
                        ).ToString()
                    )
                );
            }
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void GetReaderTest()
        {
            int expectedRecords = 50;
            for (int i = 0; i < expectedRecords; i++) {
                myNinja.NonQuery(
                    new DataNinjaQuery(
                        "INSERT INTO Test_Varchar(VarcharValue) VALUES(@InsertValue);",
                        CommandType.Text,
                        "@InsertValue", i.ToString()
                    )
                );
            }

            DataNinjaQuery readerQuery = new DataNinjaQuery(
                "SELECT TOP(@ExpectedRecords) VarcharValue FROM Test_Varchar",
                CommandType.Text,
                "@ExpectedRecords", expectedRecords
            );

            int total = 0;
            using (IDataReader reader = myNinja.GetReader(readerQuery)) {
                while (reader.Read()) {
                    total += 1;
                }
            }

            Assert.AreEqual(expectedRecords, total);
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void IsNullAndShouldBe()
        {
            Assert.AreEqual(true, DataFormatting.IsNull(null));
            Assert.AreEqual(true, DataFormatting.IsNull(DBNull.Value));
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void IsntNullAndShouldntBe()
        {
            Assert.AreEqual(false, DataFormatting.IsNull(0));
            Assert.AreEqual(false, DataFormatting.IsNull(string.Empty));
        }

        [TestMethod()]
        [TestCategory("Online")]
        public void IsNullWithDefault()
        {
            Assert.AreEqual(1, DataFormatting.IsNull(null, 1));
        }
        #endregion
    }
}