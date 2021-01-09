namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class CannotConnectException : Exception
    {
        [CompilerGenerated]
        private static Func<int, string> <>f__am$cache0;

        public CannotConnectException(string host, params int[] ports) : this($"host={host}, ports={string.Join(",", ports.Select<int, string>(<>f__am$cache0).ToArray<string>())}")
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Func<int, string>(CannotConnectException.<CannotConnectException>m__0);
            }
        }

        [CompilerGenerated]
        private static string <CannotConnectException>m__0(int port) => 
            port.ToString();
    }
}

