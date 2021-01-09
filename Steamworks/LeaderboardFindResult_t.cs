namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x450)]
    public struct LeaderboardFindResult_t
    {
        public const int k_iCallback = 0x450;
        public SteamLeaderboard_t m_hSteamLeaderboard;
        public byte m_bLeaderboardFound;
    }
}

