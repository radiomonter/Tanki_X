namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x14d)]
    public struct GameLobbyJoinRequested_t
    {
        public const int k_iCallback = 0x14d;
        public CSteamID m_steamIDLobby;
        public CSteamID m_steamIDFriend;
    }
}

