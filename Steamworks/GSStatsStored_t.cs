namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x709)]
    public struct GSStatsStored_t
    {
        public const int k_iCallback = 0x709;
        public EResult m_eResult;
        public CSteamID m_steamIDUser;
    }
}

