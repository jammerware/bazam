namespace Bazam.KeyAdept.Infrastructure
{
    public class Hotkey
    {
        private Key _Key;
        private Modifier[] _Modifiers;

        public Hotkey(Key key, params Modifier[] modifiers)
        {
            if (((int)key) == 0) {
                throw new KeyAdeptException("The key argument to Hotkey can\'t be null or 0 or whatever.");
            }

            _Key = key;
            _Modifiers = modifiers;
        }

        public Key Key
        {
            get { return _Key; }
        }

        public Modifier[] Modifiers
        {
            get { return _Modifiers; }
        }
    }
}