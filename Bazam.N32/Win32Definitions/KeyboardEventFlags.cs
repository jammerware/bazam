using System;

namespace Bazam.Win32Definitions
{
    [Flags]
    internal enum KeyboardEventFlags : uint
    {
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }
}