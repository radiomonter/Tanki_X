namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xce)]
    public struct GSClientAchievementStatus_t
    {
        public const int k_iCallback = 0xce;
        public ulong m_SteamID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
        public string m_pchAchievement;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bUnlocked;
    }
}

