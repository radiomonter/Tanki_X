namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class StatsTool
    {
        [CompilerGenerated]
        private static Func<KeyValuePair<long, long>, long> <>f__am$cache0;

        public static long GetItemWithLagestValue(Dictionary<long, long> dictionary)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = pair => pair.Value;
            }
            return dictionary.OrderBy<KeyValuePair<long, long>, long>(<>f__am$cache0).LastOrDefault<KeyValuePair<long, long>>().Key;
        }

        public static long GetParameterValue<T>(Dictionary<T, long> dictionary, T key) => 
            !dictionary.ContainsKey(key) ? 0L : dictionary[key];
    }
}

