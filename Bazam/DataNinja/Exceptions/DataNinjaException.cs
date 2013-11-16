using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bazam.DataNinja
{
    public class DataNinjaException : Exception
    {
        public DataNinjaException(string msg) : base(msg) { }
        public DataNinjaException(string methodName, Exception innerEx) : base(methodName + " - " + innerEx.Message, innerEx) { }
    }
}
