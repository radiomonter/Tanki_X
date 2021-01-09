namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1fa)]
    public struct LobbyChatUpdate_t
    {
        public const int k_iCallback = 0x1fa;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulSteamIDUserChanged;
        public ulong m_ulSteamIDMakingChange;
        public uint m_rgfChatMemberStateChange;
    }
}

