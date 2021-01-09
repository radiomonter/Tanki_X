namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x2bf)]
    public struct SteamAPICallCompleted_t
    {
        public const int k_iCallback = 0x2bf;
        public SteamAPICall_t m_hAsyncCall;
        public int m_iCallback;
        public uint m_cubParam;
    }
}

