using System.Runtime.InteropServices;
using Bazam.Win32Definitions;

namespace Bazam.N32
{
    public class N32Client
    {
        public void MouseLeftClick()
        {
            INPUT input = new INPUT()
            {
                type = InputType.MOUSE,
                U = new InputUnion()
                {
                    MouseInput = new MouseInput()
                    {
                        XCoordinate = 1,
                        YCoordinate = 1,
                        MouseEventFlags = MouseEventFlags.LEFTDOWN
                    }
                }
            };

            Methods.SendInput(1, new INPUT[] { input }, Marshal.SizeOf(input));
        }
    }
}