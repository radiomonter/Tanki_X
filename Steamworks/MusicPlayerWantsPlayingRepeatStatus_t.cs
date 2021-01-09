namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1012)]
    public struct MusicPlayerWantsPlayingRepeatStatus_t
    {
        public const int k_iCallback = 0x1012;
        public int m_nPlayingRepeatStatus;
    }
}

