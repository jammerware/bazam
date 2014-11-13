using System;

namespace Bazam.KeyAdept.Infrastructure
{
    [Flags]
    public enum Modifier
    {
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        Win = 8
    }
}