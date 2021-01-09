namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x708)]
    public struct GSStatsReceived_t
    {
        public const int k_iCallback = 0x708;
        public EResult m_eResult;
        public CSteamID m_steamIDUser;
    }
}

