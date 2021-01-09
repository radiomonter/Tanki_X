namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x458)]
    public struct GlobalStatsReceived_t
    {
        public const int k_iCallback = 0x458;
        public ulong m_nGameID;
        public EResult m_eResult;
    }
}

