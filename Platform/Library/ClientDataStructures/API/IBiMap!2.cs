namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IBiMap<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        bool Contains(TKey key, TValue value);
        void ForcePut(TKey key, TValue value);
        bool Remove(TKey key, TValue value);

        IBiMap<TValue, TKey> Inverse { get; }
    }
}

