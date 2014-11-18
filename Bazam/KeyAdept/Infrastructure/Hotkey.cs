using System.Collections.Generic;

namespace Bazam.KeyAdept.Infrastructure
{
    public class Hotkey
    {
        #region Fields
        private Key _Key;
        private ListenerWindow _ListenerWindow;
        private List<Modifier> _Modifiers;
        #endregion

        #region Events
        public event HotkeyPressedEventHandler Pressed;
        #endregion

        #region Properties
        public Key Key
        {
            get { return _Key; }
        }

        public List<Modifier> Modifiers
        {
            get { return _Modifiers; }
        }

        internal ListenerWindow ListenerWindow 
        {
            get { return _ListenerWindow; }
            set
            {
                if (_ListenerWindow != value) {
                    _ListenerWindow = value;
                    _ListenerWindow.HotkeyPressed += (HotkeyPressedEventArgs args) => {
                        if (args.Hotkey.Equals(this) && Pressed != null) {
                            Pressed(args);
                        }
                    };
                }
            }
        }
        #endregion

        public Hotkey(Key key, params Modifier[] modifiers)
        {
            if (((int)key) == 0) {
                throw new KeyAdeptException("The key argument to Hotkey can\'t be null or 0 or whatever the hell you just set it to.");
            }

            List<Modifier> mods = new List<Modifier>();
            mods.AddRange(modifiers);

            _Key = key;
            _Modifiers = mods;
        }

        #region Overridden shit
        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(Hotkey)) {
                Hotkey other = (obj as Hotkey);
                if (other.Key == this.Key && other.Modifiers.Count == this.Modifiers.Count) {
                    bool allGood = true;
                    foreach (Modifier mod in other.Modifiers) {
                        if (!this.Modifiers.Contains(mod)) {
                            allGood = false;
                            break;
                        }
                    }

                    if (allGood) return true;
                }
            }
            return false;
        }
        #endregion
    }
}