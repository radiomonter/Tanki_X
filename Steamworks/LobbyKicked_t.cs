namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x200)]
    public struct LobbyKicked_t
    {
        public const int k_iCallback = 0x200;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulSteamIDAdmin;
        public byte m_bKickedDueToDisconnect;
    }
}

