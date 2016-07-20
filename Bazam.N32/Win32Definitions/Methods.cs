using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    internal static class Methods
    {
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);
    }
}