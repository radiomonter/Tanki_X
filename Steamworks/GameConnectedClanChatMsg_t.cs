namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x152)]
    public struct GameConnectedClanChatMsg_t
    {
        public const int k_iCallback = 0x152;
        public CSteamID m_steamIDClanChat;
        public CSteamID m_steamIDUser;
        public int m_iMessageID;
    }
}

