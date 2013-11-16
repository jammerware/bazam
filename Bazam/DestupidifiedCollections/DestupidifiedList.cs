using System.Collections.Generic;
using System.Text;

namespace Bazam.DestupidifiedCollections
{
    public class DestupidifiedList<TItem> : List<TItem>
    {
        public DestupidifiedList(): base() { }

        public new bool Add(TItem item)
        {
            if (!Contains(item)) {
                base.Add(item);
                return true;
            }
            return false;
        }

        public new bool Remove(TItem item)
        {
            if (Contains(item)) {
                base.Remove(item);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return ToString(",");
        }

        public string ToString(string delimiter)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Count - 1; i++) {
                if (i > 0)
                    builder.Append(delimiter);
                builder.Append(this[i].ToString());
            }
            return builder.ToString();
        }
    }
}
