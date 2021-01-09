namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xfab)]
    public struct MusicPlayerWantsVolume_t
    {
        public const int k_iCallback = 0xfab;
        public float m_flNewVolume;
    }
}

