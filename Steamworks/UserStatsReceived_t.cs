namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Pack=8), CallbackIdentity(0x44d)]
    public struct UserStatsReceived_t
    {
        public const int k_iCallback = 0x44d;
        [FieldOffset(0)]
        public ulong m_nGameID;
        [FieldOffset(8)]
        public EResult m_eResult;
        [FieldOffset(12)]
        public CSteamID m_steamIDUser;
    }
}

