using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HardwareInput
    {
        internal int uMsg;
        internal short wParamL;
        internal short wParamH;
    }
}