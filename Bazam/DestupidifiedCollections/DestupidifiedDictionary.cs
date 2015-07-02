using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bazam.DestupidifiedCollections
{
    public class DestupidifiedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        #region Overridden Properties
        public new TValue this[TKey key] 
        {
            get { return (ContainsKey(key) ? base[key] : default(TValue)); }
            set { base[key] = value; }
        }
        #endregion

        #region Overridden Methods
        public new void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) {
                this[key] = value;
            }
            else {
                base.Add(key, value);
            }
        }

        public new bool Remove(TKey key)
        {
            if (ContainsKey(key)) {
                base.Remove(key);
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (typeof(DestupidifiedDictionary<TKey, TValue>).GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo())) {
                return IsEqualTo(this, obj as DestupidifiedDictionary<TKey, TValue>);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(",", "=");
        }

        public string ToString(string entryDelimiter, string keyValueDelimiter)
        {
            if (Count > 0) {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<TKey, TValue> pair in this) {
                    builder.Append(pair.Key.ToString());
                    builder.Append(keyValueDelimiter);
                    builder.Append(pair.Value.ToString());
                    builder.Append(entryDelimiter);
                }

                string retVal = builder.ToString();
                int computedReturnLength = retVal.Length - entryDelimiter.Length;
                if (retVal.Length > computedReturnLength)
                    retVal = retVal.Substring(0, computedReturnLength);
                return retVal;
            }

            return string.Empty;
        }
        #endregion

#region Internal Helpers
        private static bool IsEqualTo(DestupidifiedDictionary<TKey, TValue> first, DestupidifiedDictionary<TKey, TValue> second)
        {
            if (first == null || second == null)
                return false;
            if (first.Count != second.Count)
                return false;
            if(!first.Keys.Equals(second.Keys))
                return false;

            foreach(TKey key in first.Keys) {
                if (!first[key].Equals(second[key]))
                    return false;
            }
            return true;
        }
#endregion
    }
}
