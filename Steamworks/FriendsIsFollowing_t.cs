namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x159)]
    public struct FriendsIsFollowing_t
    {
        public const int k_iCallback = 0x159;
        public EResult m_eResult;
        public CSteamID m_steamID;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bIsFollowing;
    }
}

