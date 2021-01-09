namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class MultiMap<TKey, TValue> : Dictionary<TKey, HashSet<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            HashSet<TValue> set = null;
            if (!base.TryGetValue(key, out set))
            {
                set = new HashSet<TValue>();
                base.Add(key, set);
            }
            set.Add(value);
        }

        public bool ContainsValue(TValue value)
        {
            <ContainsValue>c__AnonStorey1<TKey, TValue> storey = new <ContainsValue>c__AnonStorey1<TKey, TValue> {
                value = value,
                $this = (MultiMap<TKey, TValue>) this
            };
            return base.Keys.Any<TKey>(new Func<TKey, bool>(storey.<>m__0));
        }

        public bool ContainsValue(TKey key, TValue value) => 
            base.ContainsKey(key) && base[key].Contains(value);

        public HashSet<TValue> GetAllValues() => 
            new HashSet<TValue>(from x in base.Keys select base[x]);

        public TKey GetKeyByValue(TValue value)
        {
            <GetKeyByValue>c__AnonStorey0<TKey, TValue> storey = new <GetKeyByValue>c__AnonStorey0<TKey, TValue> {
                value = value,
                $this = (MultiMap<TKey, TValue>) this
            };
            return base.Keys.Where<TKey>(new Func<TKey, bool>(storey.<>m__0)).First<TKey>();
        }

        public HashSet<TValue> GetValues(TKey key)
        {
            HashSet<TValue> set;
            base.TryGetValue(key, out set);
            return (set ?? new HashSet<TValue>());
        }

        public void Merge(MultiMap<TKey, TValue> toMergeWith)
        {
            if (toMergeWith != null)
            {
                Dictionary<TKey, HashSet<TValue>>.Enumerator enumerator = toMergeWith.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<TKey, HashSet<TValue>> current = enumerator.Current;
                    HashSet<TValue>.Enumerator enumerator2 = current.Value.GetEnumerator();
                    TKey key = current.Key;
                    while (enumerator2.MoveNext())
                    {
                        this.Add(key, enumerator2.Current);
                    }
                }
            }
        }

        public void Remove(TKey key, TValue value)
        {
            if (base.ContainsKey(key))
            {
                base[key].Remove(value);
                if (base[key].Count == 0)
                {
                    base.Remove(key);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ContainsValue>c__AnonStorey1
        {
            internal TValue value;
            internal MultiMap<TKey, TValue> $this;

            internal bool <>m__0(TKey x) => 
                this.$this[x].Contains(this.value);
        }

        [CompilerGenerated]
        private sealed class <GetKeyByValue>c__AnonStorey0
        {
            internal TValue value;
            internal MultiMap<TKey, TValue> $this;

            internal bool <>m__0(TKey x) => 
                this.$this[x].Contains(this.value);
        }
    }
}

