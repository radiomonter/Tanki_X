namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableExtension
    {
        public static void ForEach<TKey>(this IEnumerable<TKey> enumerable, Action<TKey> compute)
        {
            Collections.ForEach<TKey>(enumerable, compute);
        }

        public static void ForEach<TKey>(this IList<TKey> list, Action<TKey> compute)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                compute(list[i]);
            }
        }
    }
}

