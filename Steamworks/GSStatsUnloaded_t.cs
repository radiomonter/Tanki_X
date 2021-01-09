namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x454)]
    public struct GSStatsUnloaded_t
    {
        public const int k_iCallback = 0x454;
        public CSteamID m_steamIDUser;
    }
}

