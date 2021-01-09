namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1f9)]
    public struct LobbyDataUpdate_t
    {
        public const int k_iCallback = 0x1f9;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulSteamIDMember;
        public byte m_bSuccess;
    }
}

