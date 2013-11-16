using System;
using System.Collections.Generic;
using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BazamTests
{
    [TestClass]
    public class ListlessTest
    {
        [TestMethod]
        [TestCategory("Offline")]
        public void WorksWithComma()
        {
            List<int> ints = new List<int>();
            ints.Add(1);
            ints.Add(2);
            ints.Add(3);

            Assert.AreEqual("1,2,3", Listless.ListToString(ints)); 
        }

        [TestMethod]
        [TestCategory("Offline")]
        public void WorksWithSomethingLonger()
        {
            List<int> ints = new List<int>();
            ints.Add(1);
            ints.Add(2);
            ints.Add(3);

            Assert.AreEqual("1BOOGER2BOOGER3", Listless.ListToString(ints, "BOOGER"));
        }
    }
}