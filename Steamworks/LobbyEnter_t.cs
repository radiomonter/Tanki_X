namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1f8)]
    public struct LobbyEnter_t
    {
        public const int k_iCallback = 0x1f8;
        public ulong m_ulSteamIDLobby;
        public uint m_rgfChatPermissions;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bLocked;
        public uint m_EChatRoomEnterResponse;
    }
}

