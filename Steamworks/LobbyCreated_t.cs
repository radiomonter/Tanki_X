namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x201)]
    public struct LobbyCreated_t
    {
        public const int k_iCallback = 0x201;
        public EResult m_eResult;
        public ulong m_ulSteamIDLobby;
    }
}

