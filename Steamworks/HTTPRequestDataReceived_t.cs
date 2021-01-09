namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x837)]
    public struct HTTPRequestDataReceived_t
    {
        public const int k_iCallback = 0x837;
        public HTTPRequestHandle m_hRequest;
        public ulong m_ulContextValue;
        public uint m_cOffset;
        public uint m_cBytesReceived;
    }
}

