using System;

namespace Bazam.KeyAdept.Infrastructure
{
    public class HotkeyEventArgs : EventArgs
    {
        public Hotkey Hotkey { get; protected set; }

        public HotkeyEventArgs(Hotkey hotkey)
        {
            this.Hotkey = hotkey;
        }
    }
}