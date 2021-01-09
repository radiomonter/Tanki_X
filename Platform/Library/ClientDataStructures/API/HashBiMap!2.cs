namespace Platform.Library.ClientDataStructures.API
{
    using Platform.Library.ClientDataStructures.Impl;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class HashBiMap<TKey, TValue> : IBiMap<TKey, TValue>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private Dictionary<TKey, TValue> data;
        private InverseBiMap<TKey, TValue, TValue, TKey> inverse;

        public HashBiMap()
        {
            this.data = new Dictionary<TKey, TValue>();
            this.inverse = new InverseBiMap<TKey, TValue, TValue, TKey>(this);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            this.CheckNotNulls(key, value);
            if (this.data.ContainsKey(key))
            {
                throw new KeyAlreadyExistsException(key);
            }
            if (this.inverse.inverseData.ContainsKey(value))
            {
                throw new ValueAlreadyExistsException(value);
            }
            this.data.Add(key, value);
            this.inverse.inverseData.Add(value, key);
        }

        private void CheckNotNulls(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new KeyIsNullExcpetion();
            }
            if (value == null)
            {
                throw new ValueIsNullException();
            }
        }

        public void Clear()
        {
            this.data.Clear();
            this.inverse.inverseData.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => 
            this.Contains(item.Key, item.Value);

        public bool Contains(TKey key, TValue value)
        {
            TValue local;
            return (this.data.TryGetValue(key, out local) && Equals(value, local));
        }

        public bool ContainsKey(TKey key) => 
            this.data.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public void ForcePut(TKey key, TValue value)
        {
            TValue local;
            TKey local2;
            this.CheckNotNulls(key, value);
            if (this.data.TryGetValue(key, out local))
            {
                this.data.Remove(key);
                this.inverse.inverseData.Remove(local);
            }
            if (this.inverse.inverseData.TryGetValue(value, out local2))
            {
                this.inverse.inverseData.Remove(value);
                this.data.Remove(local2);
            }
            this.data.Add(key, value);
            this.inverse.inverseData.Add(value, key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
            this.data.GetEnumerator();

        public bool Remove(TKey key)
        {
            TValue local;
            if (!this.data.TryGetValue(key, out local))
            {
                return false;
            }
            this.data.Remove(key);
            this.inverse.inverseData.Remove(local);
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => 
            this.Remove(item.Key, item.Value);

        public bool Remove(TKey key, TValue value)
        {
            TValue local;
            if (!this.data.TryGetValue(key, out local) || !Equals(value, local))
            {
                return false;
            }
            this.data.Remove(key);
            this.inverse.inverseData.Remove(value);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => 
            this.data.TryGetValue(key, out value);

        public int Count =>
            this.data.Count;

        public IBiMap<TValue, TKey> Inverse =>
            this.inverse;

        public bool IsReadOnly =>
            false;

        public ICollection<TKey> Keys =>
            this.data.Keys;

        public ICollection<TValue> Values =>
            this.data.Values;

        public TValue this[TKey key]
        {
            get
            {
                TValue local;
                if (!this.data.TryGetValue(key, out local))
                {
                    throw new KeyNotFoundException(key);
                }
                return local;
            }
            set => 
                this.ForcePut(key, value);
        }

        private class InverseBiMap<TValue, TKey> : IBiMap<TValue, TKey>, IDictionary<TValue, TKey>, ICollection<KeyValuePair<TValue, TKey>>, IEnumerable<KeyValuePair<TValue, TKey>>, IEnumerable
        {
            internal Dictionary<TValue, TKey> inverseData;
            private IBiMap<TKey, TValue> direct;

            public InverseBiMap(IBiMap<TKey, TValue> direct)
            {
                this.inverseData = new Dictionary<TValue, TKey>();
                this.direct = direct;
            }

            public void Add(KeyValuePair<TValue, TKey> item)
            {
                this.direct.Add(item.Value, item.Key);
            }

            public void Add(TValue value, TKey key)
            {
                this.direct.Add(key, value);
            }

            public void Clear()
            {
                this.direct.Clear();
            }

            public bool Contains(KeyValuePair<TValue, TKey> item) => 
                this.direct.Contains(item.Value, item.Key);

            public bool Contains(TValue value, TKey key) => 
                this.direct.Contains(key, value);

            public bool ContainsKey(TValue value) => 
                this.inverseData.ContainsKey(value);

            public void CopyTo(KeyValuePair<TValue, TKey>[] array, int arrayIndex)
            {
                this.inverseData.CopyTo(array, arrayIndex);
            }

            public void ForcePut(TValue value, TKey key)
            {
                this.direct.ForcePut(key, value);
            }

            public IEnumerator<KeyValuePair<TValue, TKey>> GetEnumerator() => 
                (IEnumerator<KeyValuePair<TValue, TKey>>) this.inverseData.GetEnumerator();

            public bool Remove(TValue value)
            {
                TKey local;
                return (this.inverseData.TryGetValue(value, out local) && this.direct.Remove(local));
            }

            public bool Remove(KeyValuePair<TValue, TKey> item) => 
                this.direct.Remove(item.Value, item.Key);

            public bool Remove(TValue value, TKey key) => 
                this.direct.Remove(key, value);

            IEnumerator IEnumerable.GetEnumerator() => 
                this.GetEnumerator();

            public bool TryGetValue(TValue value, out TKey key) => 
                this.inverseData.TryGetValue(value, out key);

            public int Count =>
                this.direct.Count;

            public IBiMap<TKey, TValue> Inverse =>
                this.direct;

            public bool IsReadOnly =>
                false;

            public TKey this[TValue _value]
            {
                get
                {
                    TKey local;
                    if (!this.inverseData.TryGetValue(_value, out local))
                    {
                        throw new ValueNotFoundException(_value);
                    }
                    return local;
                }
                set => 
                    this.direct.ForcePut(value, _value);
            }

            public ICollection<TValue> Keys =>
                (ICollection<TValue>) this.inverseData.Keys;

            public ICollection<TKey> Values =>
                (ICollection<TKey>) this.inverseData.Values;
        }
    }
}

