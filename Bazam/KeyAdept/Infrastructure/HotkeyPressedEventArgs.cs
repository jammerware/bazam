using System;

namespace Bazam.KeyAdept.Infrastructure
{
    public class HotkeyPressedEventArgs : EventArgs
    {
        Hotkey _Hotkey;

        public Hotkey Hotkey
        {
            get { return _Hotkey; }
        }

        public HotkeyPressedEventArgs(Hotkey hotkey)
        {
            _Hotkey = hotkey;
        }
    }
}
