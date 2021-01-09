namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x153)]
    public struct GameConnectedChatJoin_t
    {
        public const int k_iCallback = 0x153;
        public CSteamID m_steamIDClanChat;
        public CSteamID m_steamIDUser;
    }
}

