namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd49)]
    public struct SteamUGCQueryCompleted_t
    {
        public const int k_iCallback = 0xd49;
        public UGCQueryHandle_t m_handle;
        public EResult m_eResult;
        public uint m_unNumResultsReturned;
        public uint m_unTotalMatchingResults;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bCachedData;
    }
}

