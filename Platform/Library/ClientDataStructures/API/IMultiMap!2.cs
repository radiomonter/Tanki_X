namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMultiMap<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        void Add(TKey key, TValue value);
        void AddAll(TKey key, ICollection<TValue> values);
        bool Contains(TKey key, TValue value);
        bool ContainsKey(TKey key);
        bool ContainsValue(TValue value);
        ICollection<KeyValuePair<TKey, TValue>> Entries();
        bool Remove(TKey key, TValue value);
        ICollection<TValue> RemoveAll(TKey key);

        ICollection<TValue> this[TKey key] { get; set; }

        ICollection<TKey> Keys { get; }

        ICollection<TValue> Values { get; }
    }
}

