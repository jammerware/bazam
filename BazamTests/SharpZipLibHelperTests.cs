using System;
using Bazam.APIs.SharpZipLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BazamTests
{
    [TestClass]
    public class SharpZipLibHelperTests
    {
        [TestMethod]
        public void DoesItFuckingWork()
        {
            SharpZipLibHelper.Unzip("C:\\Users\\Jammer\\AppData\\Roaming\\Jammerware.MtGBar.Test\\packages\\core.zip", "C:\\Users\\Jammer\\AppData\\Roaming\\Jammerware.MtGBar.Test\\packages", "jammerware.isthebest", true);
        }
    }
}