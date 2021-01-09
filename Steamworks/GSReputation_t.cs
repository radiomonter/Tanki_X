namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd1)]
    public struct GSReputation_t
    {
        public const int k_iCallback = 0xd1;
        public EResult m_eResult;
        public uint m_unReputationScore;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bBanned;
        public uint m_unBannedIP;
        public ushort m_usBannedPort;
        public ulong m_ulBannedGameID;
        public uint m_unBanExpires;
    }
}

