namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd4a)]
    public struct SteamUGCRequestUGCDetailsResult_t
    {
        public const int k_iCallback = 0xd4a;
        public SteamUGCDetails_t m_details;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bCachedData;
    }
}

