namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1f7)]
    public struct LobbyInvite_t
    {
        public const int k_iCallback = 0x1f7;
        public ulong m_ulSteamIDUser;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulGameID;
    }
}

