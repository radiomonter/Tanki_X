namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct FriendGameInfo_t
    {
        public CGameID m_gameID;
        public uint m_unGameIP;
        public ushort m_usGamePort;
        public ushort m_usQueryPort;
        public CSteamID m_steamIDLobby;
    }
}

