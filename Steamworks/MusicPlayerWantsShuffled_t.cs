namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x100d)]
    public struct MusicPlayerWantsShuffled_t
    {
        public const int k_iCallback = 0x100d;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bShuffled;
    }
}

