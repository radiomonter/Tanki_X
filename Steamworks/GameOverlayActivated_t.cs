namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x14b)]
    public struct GameOverlayActivated_t
    {
        public const int k_iCallback = 0x14b;
        public byte m_bActive;
    }
}

