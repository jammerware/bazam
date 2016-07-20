using System;
using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardInput
    {
        internal VirtualKey VirtualKey;
        internal ScanCode ScanCode;
        internal KeyboardEventFlags KeyboardEventFlags;
        internal int Time;
        internal UIntPtr dwExtraInfo;
    }
}