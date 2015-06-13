using System.Linq;
using System.Xml.Linq;
using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BazamTests
{
    [TestClass]
    public class XmlPalTest
    {
        private string _XmlText = @"<root><child stringAttr=""some stuff"" intAttr=""17"" dateAttr=""06/13/2015""/></root>";

        [TestMethod]
        public void GetStringRuns()
        {
            XDocument doc = XDocument.Parse(_XmlText);
            string stringAttr = XmlPal.GetString(doc.Root.Element("child").Attribute("stringAttr"));
        }

        [TestMethod]
        public void CheckThatWeirdStackOverflowBugOops()
        {
            XDocument doc = XDocument.Load("Assets/core.xml");

            bool[] raw = (
                from set in doc.Root.Element("sets").Elements("set")
                select (XmlPal.GetBool(set.Element("is_promo")) ?? false)
            ).ToArray();
        }
    }
}