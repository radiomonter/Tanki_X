namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xfa2)]
    public struct VolumeHasChanged_t
    {
        public const int k_iCallback = 0xfa2;
        public float m_flNewVolume;
    }
}

