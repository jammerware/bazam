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

        public event UnavailableHotkeyRegisteredEventHandler UnavailableHotkeyRegistered;
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

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            bool retVal = true;
            int hotkeyID = RegisterHotkey(hotkey, null, true);

            if (hotkeyID < 0) retVal = false;
            else UnregisterHotkey(hotkeyID);

            return retVal;
        }

        private int RegisterHotkey(Hotkey hotkey, int? id, bool suppressEvent)
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
            foreach (Modifier mod in hotkey.Modifiers) {
                modifiers |= (uint)mod;
            }

            if (RegisterHotKey(_Window.Handle, idValue, modifiers, (uint)hotkey.Key)) {
                hotkey.ListenerWindow = _Window;
                hotkey.Pressed += (HotkeyPressedEventArgs args) => {
                    if (HotkeyPressed != null) {
                        HotkeyPressed(args);
                    }
                };
                _Hotkeys.Add(idValue, hotkey);
            }
            else {
                if (!suppressEvent && UnavailableHotkeyRegistered != null) {
                    UnavailableHotkeyRegistered(new HotkeyEventArgs(hotkey));
                }
                idValue = -1;
            }
            return idValue;
        }

        // returns the ID of the registered hotkey
        public int RegisterHotkey(Hotkey hotkey, int? id)
        {
            return RegisterHotkey(hotkey, id, false);
        }

        public void RegisterHotkey(Hotkey hotkey)
        {
            RegisterHotkey(hotkey, null, false);
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