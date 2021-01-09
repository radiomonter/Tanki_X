namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class HashMultiMap<TKey, TValue> : IMultiMap<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private int count;
        private int version;
        private Dictionary<TKey, ICollection<TValue>> data;

        public HashMultiMap()
        {
            this.data = new Dictionary<TKey, ICollection<TValue>>();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            ICollection<TValue> is2;
            if (!this.data.TryGetValue(key, out is2))
            {
                is2 = this.CreateValueCollection();
                this.data.Add(key, is2);
            }
            is2.Add(value);
            this.count++;
            this.version++;
        }

        public void AddAll(TKey key, ICollection<TValue> values)
        {
            ICollection<TValue> is2;
            if (!this.data.TryGetValue(key, out is2))
            {
                is2 = this.CreateValueCollection();
                this.data.Add(key, is2);
            }
            foreach (TValue local in values)
            {
                is2.Add(local);
                this.count++;
            }
            this.version++;
        }

        public void Clear()
        {
            this.data.Clear();
            this.count = 0;
            this.version++;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => 
            this.Contains(item.Key, item.Value);

        public bool Contains(TKey key, TValue value)
        {
            ICollection<TValue> is2;
            return (this.data.TryGetValue(key, out is2) && is2.Contains(value));
        }

        public bool ContainsKey(TKey key) => 
            this.data.ContainsKey(key);

        public bool ContainsValue(TValue value)
        {
            bool flag;
            using (Dictionary<TKey, ICollection<TValue>>.KeyCollection.Enumerator enumerator = this.data.Keys.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        TKey current = enumerator.Current;
                        if (!this.data[current].Contains(value))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.Entries().CopyTo(array, arrayIndex);
        }

        protected virtual ICollection<TValue> CreateValueCollection() => 
            new List<TValue>();

        public ICollection<KeyValuePair<TKey, TValue>> Entries()
        {
            List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>(this.count);
            foreach (KeyValuePair<TKey, ICollection<TValue>> pair in this.data)
            {
                foreach (TValue local in pair.Value)
                {
                    list.Add(new KeyValuePair<TKey, TValue>(pair.Key, local));
                }
            }
            return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(list);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
            new Enumerator<TKey, TValue>((HashMultiMap<TKey, TValue>) this);

        public bool Remove(KeyValuePair<TKey, TValue> item) => 
            this.Remove(item.Key, item.Value);

        public bool Remove(TKey key, TValue value)
        {
            ICollection<TValue> is2;
            if (!this.data.TryGetValue(key, out is2))
            {
                return false;
            }
            bool flag = is2.Remove(value);
            if (flag)
            {
                if (is2.Count == 0)
                {
                    this.data.Remove(key);
                }
                this.count--;
                this.version++;
            }
            return flag;
        }

        public ICollection<TValue> RemoveAll(TKey key)
        {
            ICollection<TValue> is2;
            if (!this.data.TryGetValue(key, out is2))
            {
                return new EmptyCollection<TValue>();
            }
            this.count -= is2.Count;
            this.data.Remove(key);
            this.version++;
            return is2;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            this.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            new Enumerator<TKey, TValue>((HashMultiMap<TKey, TValue>) this);

        public int Count =>
            this.count;

        public ICollection<TValue> this[TKey key]
        {
            get
            {
                ICollection<TValue> is2;
                return (!this.data.TryGetValue(key, out is2) ? ((ICollection<TValue>) Collections.EmptyList<TValue>()) : is2);
            }
            set => 
                this.data[key] = value;
        }

        public ICollection<TKey> Keys =>
            this.data.Keys;

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> list = new List<TValue>();
                foreach (ICollection<TValue> is2 in this.data.Values)
                {
                    list.AddRange(is2);
                }
                return new ReadOnlyCollection<TValue>(list);
            }
        }

        public bool IsReadOnly =>
            false;

        [StructLayout(LayoutKind.Sequential)]
        private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator
        {
            private HashMultiMap<TKey, TValue> multimap;
            private IEnumerator<TKey> keyEnumerator;
            private IEnumerator<TValue> valueEnumerator;
            private int ver;
            private EnumeratorHelper.EnumeratorState state;
            public Enumerator(HashMultiMap<TKey, TValue> multimap)
            {
                this.multimap = multimap;
                this.ver = multimap.version;
                this.state = EnumeratorHelper.EnumeratorState.Before;
                this.keyEnumerator = null;
                this.valueEnumerator = null;
            }

            public bool MoveNext()
            {
                bool flag;
                EnumeratorHelper.CheckVersion(this.ver, this.multimap.version);
                EnumeratorHelper.EnumeratorState state = this.state;
                if (state == EnumeratorHelper.EnumeratorState.Before)
                {
                    this.keyEnumerator = this.multimap.data.Keys.GetEnumerator();
                    flag = this.keyEnumerator.MoveNext();
                    if (!flag)
                    {
                        this.state = EnumeratorHelper.EnumeratorState.After;
                    }
                    else
                    {
                        this.valueEnumerator = this.multimap.data[this.keyEnumerator.Current].GetEnumerator();
                        this.valueEnumerator.MoveNext();
                        this.state = EnumeratorHelper.EnumeratorState.Current;
                    }
                    return flag;
                }
                if (state == EnumeratorHelper.EnumeratorState.After)
                {
                    return false;
                }
                flag = this.valueEnumerator.MoveNext();
                if (!flag)
                {
                    flag = this.keyEnumerator.MoveNext();
                    if (flag)
                    {
                        this.valueEnumerator = this.multimap.data[this.keyEnumerator.Current].GetEnumerator();
                        this.valueEnumerator.MoveNext();
                    }
                }
                if (!flag)
                {
                    this.state = EnumeratorHelper.EnumeratorState.After;
                }
                return flag;
            }

            public void Reset()
            {
                this.state = EnumeratorHelper.EnumeratorState.Before;
            }

            object IEnumerator.Current =>
                this.Current;
            public void Dispose()
            {
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    EnumeratorHelper.CheckCurrentState(this.state);
                    return new KeyValuePair<TKey, TValue>(this.keyEnumerator.Current, this.valueEnumerator.Current);
                }
            }
        }
    }
}

