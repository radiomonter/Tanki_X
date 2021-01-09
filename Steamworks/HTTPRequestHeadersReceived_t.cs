namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x836)]
    public struct HTTPRequestHeadersReceived_t
    {
        public const int k_iCallback = 0x836;
        public HTTPRequestHandle m_hRequest;
        public ulong m_ulContextValue;
    }
}

