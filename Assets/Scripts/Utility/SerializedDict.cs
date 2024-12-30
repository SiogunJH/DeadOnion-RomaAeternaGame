using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    // stolen from VInspector<3
    
    [System.Serializable]
    public class SerializedDict<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    { 
        public List<SerializedKeyValuePair<TKey, TValue>> Pairs = new();

        public void OnBeforeSerialize()
        {
            foreach (var kvp in this)
            {
                if (Pairs.FirstOrDefault(r => this.Comparer.Equals(r.Key, kvp.Key)) is SerializedKeyValuePair<TKey, TValue> serializedKvp)
                    serializedKvp.Value = kvp.Value;
                else
                    Pairs.Add(kvp);
            }

            Pairs.RemoveAll(r => !this.ContainsKey(r.Key));
        }
        
        public void OnAfterDeserialize()
        {
            this.Clear();

            Pairs.RemoveAll(r => r.Key == null);

            foreach (var serializedKvp in Pairs)
            {
                this.Add(serializedKvp.Key, serializedKvp.Value);
            }
        }

        [System.Serializable]
        public class SerializedKeyValuePair<TKey_, TValue_>
        {
            public TKey_ Key;
            public TValue_ Value;
            public SerializedKeyValuePair(TKey_ key, TValue_ value) { this.Key = key; this.Value = value; }

            public static implicit operator SerializedKeyValuePair<TKey_, TValue_>(KeyValuePair<TKey_, TValue_> kvp) => new(kvp.Key, kvp.Value);
            public static implicit operator KeyValuePair<TKey_, TValue_>(SerializedKeyValuePair<TKey_, TValue_> kvp) => new(kvp.Key, kvp.Value);
        }
    }
}