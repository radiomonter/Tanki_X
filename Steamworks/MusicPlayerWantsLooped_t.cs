namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x100e)]
    public struct MusicPlayerWantsLooped_t
    {
        public const int k_iCallback = 0x100e;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bLooped;
    }
}

