using System;

namespace Bazam.KeyAdept.Infrastructure
{
    public class HotkeyPressedEventArgs : HotkeyEventArgs
    {
        public HotkeyPressedEventArgs(Hotkey hotkey) : base(hotkey) { }
    }
}