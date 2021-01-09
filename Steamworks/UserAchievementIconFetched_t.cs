namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x455)]
    public struct UserAchievementIconFetched_t
    {
        public const int k_iCallback = 0x455;
        public CGameID m_nGameID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
        public string m_rgchAchievementName;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bAchieved;
        public int m_nIconHandle;
    }
}

