namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x44f)]
    public struct UserAchievementStored_t
    {
        public const int k_iCallback = 0x44f;
        public ulong m_nGameID;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bGroupAchievement;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
        public string m_rgchAchievementName;
        public uint m_nCurProgress;
        public uint m_nMaxProgress;
    }
}

