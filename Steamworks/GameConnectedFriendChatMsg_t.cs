namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x157)]
    public struct GameConnectedFriendChatMsg_t
    {
        public const int k_iCallback = 0x157;
        public CSteamID m_steamIDUser;
        public int m_iMessageID;
    }
}

