using System;

namespace Bazam.Modules.Enumerations
{
    public class EnuMasterException : Exception
    {
        public EnuMasterException(string message) : base(message) { }
        public EnuMasterException(string message, Exception innerEx) : base(message, innerEx) { }
    }
}