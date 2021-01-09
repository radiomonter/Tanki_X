namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x452)]
    public struct LeaderboardScoreUploaded_t
    {
        public const int k_iCallback = 0x452;
        public byte m_bSuccess;
        public SteamLeaderboard_t m_hSteamLeaderboard;
        public int m_nScore;
        public byte m_bScoreChanged;
        public int m_nGlobalRankNew;
        public int m_nGlobalRankPrevious;
    }
}

