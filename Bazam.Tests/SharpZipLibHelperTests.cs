using System;
using Bazam.SharpZipLibHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazam.Tests
{
    [TestClass]
    public class SharpZipLibHelperTests
    {
        [TestMethod]
        public void DoesUnzipWork()
        {
            SharpZipLibHelper.Unzip(
                "Assets/dd3.gbd", "UnzipOutput", "jammerware.isthebest", true
            );
        }
    }
}