using System.Runtime.InteropServices;

namespace Bazam.Win32Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        internal InputType type;
        internal InputUnion U;

        internal static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }
}