using System.Collections.Generic;
using System.Linq;
using Bazam.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazam.Tests
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        private string[] _SomeOptions = new string[] { "Duh", "And", "Or", "Hello", "LANAAAAAAA!" };

        [TestMethod]
        public void MySyntaxIsActuallyCorrect()
        {
            Assert.IsTrue(_SomeOptions.Contains(_SomeOptions.Random()));
        }

        // TODO: this is weird. the test passes if i put a breakpoint inside the loop and just hammer 
        // F5, but if i run it normally it fails. wut. check into it, yo.
        [TestMethod]
        public void LiterallyTheWorstTestOfARandomizerEver()
        {
            List<string> listOptions = _SomeOptions.ToList();

            for (int i = 0; i < 100; i++) {
                string randomResult = _SomeOptions.Random();
                if (listOptions.Contains(randomResult)) {
                    listOptions.Remove(randomResult);

                    if(listOptions.Count == 0) {
                        break;
                    }
                }
            }

            Assert.AreEqual(0, listOptions.Count);
        }
    }
}