namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x156)]
    public struct JoinClanChatRoomCompletionResult_t
    {
        public const int k_iCallback = 0x156;
        public CSteamID m_steamIDClanChat;
        public EChatRoomEnterResponse m_eChatRoomEnterResponse;
    }
}

