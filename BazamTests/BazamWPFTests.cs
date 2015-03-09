using System;
using System.Globalization;
using System.Windows;
using BazamWPF.ValueConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BazamTests
{
    [TestClass]
    public class BazamWPFTests
    {
        [TestMethod]
        public void BooleanVisibilityConverterWorks()
        {
            BooleanVisibilityConverter converter = new BooleanVisibilityConverter();
            object result = converter.Convert(false, typeof(Visibility), "true", CultureInfo.CurrentCulture);
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}