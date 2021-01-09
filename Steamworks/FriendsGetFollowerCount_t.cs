namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x158)]
    public struct FriendsGetFollowerCount_t
    {
        public const int k_iCallback = 0x158;
        public EResult m_eResult;
        public CSteamID m_steamID;
        public int m_nCount;
    }
}

