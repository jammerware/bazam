using System;

namespace Bazam.KeyAdept.Infrastructure
{
    public class KeyAdeptException : Exception
    {
        public KeyAdeptException(string message) : base(message) {}
        public KeyAdeptException(Exception ex) : base("KeyAdept threw an exception.", ex) {}
    }
}