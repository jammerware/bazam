using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Bazam.KeyAdept.Infrastructure;

namespace Bazam.KeyAdept
{
    public class HotkeyRegistrar
    {
        private int _DefaultID = 0;
        private Dictionary<int, Hotkey> _Hotkeys;
        private ListenerWindow _Window;

        public event HotkeyPressedEventHandler HotkeyPressed;

        public HotkeyRegistrar()
        {
            _Hotkeys = new Dictionary<int, Hotkey>();
            _Window = new ListenerWindow();
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // returns the ID of the registered hotkey
        public int RegisterHotkey(Hotkey hotkey, int? id)
       { 
            int idValue = _DefaultID;

            if (id == null) {
                while (_Hotkeys.ContainsKey(_DefaultID)) {
                    _DefaultID++;
                }
                idValue = _DefaultID;
                _DefaultID++;
            }

            uint modifiers = 0;
            foreach(Modifier mod in hotkey.Modifiers) {
                modifiers |= (uint)mod;
            }

            try {
                RegisterHotKey(_Window.Handle, idValue, modifiers, (uint)hotkey.Key);

                hotkey.ListenerWindow = _Window;
                hotkey.Pressed += (HotkeyPressedEventArgs args) => {
                    if (HotkeyPressed != null) {
                        HotkeyPressed(args);
                    }
                };
                _Hotkeys.Add(idValue, hotkey);
            }
            catch (Exception ex) {
                throw new KeyAdeptException(ex);
            }

            return idValue;
        }

        public void RegisterHotkey(Hotkey hotkey)
        {
            RegisterHotkey(hotkey, null);
        }

        public void UnregisterAllHotkeys()
        {
            foreach (int id in _Hotkeys.Keys) {
                UnregisterHotkey(id);
            }
        }

        public void UnregisterHotkey(int id)
        {
            if(!_Hotkeys.ContainsKey(id)) {
                throw new KeyAdeptException("No hotkeys with ID " + id.ToString() + " have been registered.");
            }

            UnregisterHotKey(_Window.Handle, id);
        }
    }
}