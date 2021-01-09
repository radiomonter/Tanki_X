namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x150)]
    public struct FriendRichPresenceUpdate_t
    {
        public const int k_iCallback = 0x150;
        public CSteamID m_steamIDFriend;
        public AppId_t m_nAppID;
    }
}

