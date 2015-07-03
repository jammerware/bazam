using System.Xml.Linq;
using Bazam.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazam.Tests
{
    [TestClass]
    public class XmlPalTest
    {
        [TestMethod]
        public void CheckWeirdStackOverflowBiz()
        {
            XDocument doc = XDocument.Load("Assets/core.xml");
            foreach (XElement setElement in doc.Root.Element("sets").Elements("set")) {
                bool isPromo = (XmlPal.GetBool(setElement.Element("is_promo")) ?? false);                
            }
        }
    }
}