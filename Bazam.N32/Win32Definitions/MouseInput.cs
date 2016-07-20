using System;
using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseInput
    {
        internal int XCoordinate;
        internal int YCoordinate;
        internal int MouseData;
        internal MouseEventFlags MouseEventFlags;
        internal uint Time;
        internal UIntPtr dwExtraInfo;
    }
}