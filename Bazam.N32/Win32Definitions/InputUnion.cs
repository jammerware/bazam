using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        internal MouseInput MouseInput;

        [FieldOffset(0)]
        internal KeyboardInput KeyboardInput;

        [FieldOffset(0)]
        internal HardwareInput HardwareInput;
    }
}