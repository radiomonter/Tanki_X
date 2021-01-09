namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x456)]
    public struct GlobalAchievementPercentagesReady_t
    {
        public const int k_iCallback = 0x456;
        public ulong m_nGameID;
        public EResult m_eResult;
    }
}

