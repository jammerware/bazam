using System;
using System.Windows.Forms;

namespace Bazam.KeyAdept.Infrastructure
{
    /// <summary>
    /// Represents the window that is used internally to get the messages.
    /// </summary>
    public class ListenerWindow : NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0X0312;
        public event HotkeyPressedEventHandler HotkeyPressed;

        public ListenerWindow()
        {
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Overridden to get the notifications.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // check if we got a hot key pressed.
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Key key = (Key)(((int)m.LParam >> 16) & 0xFFFF);
                Modifier modifier = (Modifier)((int)m.LParam & 0xFFFF);
                Hotkey hotkey = new Hotkey(key, modifier);

                // invoke the event to notify the parent.
                if (HotkeyPressed != null)
                    HotkeyPressed(new HotkeyPressedEventArgs(hotkey));
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.DestroyHandle();
        }
        #endregion
    }
}