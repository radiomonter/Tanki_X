namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1fb)]
    public struct LobbyChatMsg_t
    {
        public const int k_iCallback = 0x1fb;
        public ulong m_ulSteamIDLobby;
        public ulong m_ulSteamIDUser;
        public byte m_eChatEntryType;
        public uint m_iChatID;
    }
}

