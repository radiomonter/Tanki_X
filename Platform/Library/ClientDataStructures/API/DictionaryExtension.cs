namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class DictionaryExtension
    {
        public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> compute)
        {
            TValue local;
            if (dictionary.TryGetValue(key, out local))
            {
                return local;
            }
            TValue local2 = compute(key);
            dictionary.Add(key, local2);
            return local2;
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> compute)
        {
            TValue local;
            return (!dictionary.TryGetValue(key, out local) ? compute() : local);
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue local;
            return (!dictionary.TryGetValue(key, out local) ? defaultValue : local);
        }
    }
}

