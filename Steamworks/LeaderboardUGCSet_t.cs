namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x457)]
    public struct LeaderboardUGCSet_t
    {
        public const int k_iCallback = 0x457;
        public EResult m_eResult;
        public SteamLeaderboard_t m_hSteamLeaderboard;
    }
}

