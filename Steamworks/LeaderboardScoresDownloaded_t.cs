namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x451)]
    public struct LeaderboardScoresDownloaded_t
    {
        public const int k_iCallback = 0x451;
        public SteamLeaderboard_t m_hSteamLeaderboard;
        public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;
        public int m_cEntryCount;
    }
}

