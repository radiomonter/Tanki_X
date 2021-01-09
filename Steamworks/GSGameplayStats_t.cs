namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xcf)]
    public struct GSGameplayStats_t
    {
        public const int k_iCallback = 0xcf;
        public EResult m_eResult;
        public int m_nRank;
        public uint m_unTotalConnects;
        public uint m_unTotalMinutesPlayed;
    }
}

