using System;
using System.Globalization;
using System.Windows;
using Bazam.Wpf.ValueConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazam.Tests
{
    [TestClass]
    public class BazamWpfTests
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