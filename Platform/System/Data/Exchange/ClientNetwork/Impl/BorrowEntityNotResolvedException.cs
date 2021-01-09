namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class BorrowEntityNotResolvedException : Exception
    {
        [CompilerGenerated]
        private static Converter<long, string> <>f__am$cache0;

        public BorrowEntityNotResolvedException(long[] ids) : this("Entity id=" + string.Join(", ", Array.ConvertAll<long, string>(ids, <>f__am$cache0)))
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Converter<long, string>(BorrowEntityNotResolvedException.<BorrowEntityNotResolvedException>m__0);
            }
        }

        [CompilerGenerated]
        private static string <BorrowEntityNotResolvedException>m__0(long s) => 
            s.ToString();
    }
}

