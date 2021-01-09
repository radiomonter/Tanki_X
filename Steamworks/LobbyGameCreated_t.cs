namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1fd)]
    public struct LobbyGameCreated_t
    {
        public const int k_iCallback = 0x1fd;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulSteamIDGameServer;
        public uint m_unIP;
        public ushort m_usPort;
    }
}

