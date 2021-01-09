namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x8fd)]
    public struct ScreenshotReady_t
    {
        public const int k_iCallback = 0x8fd;
        public ScreenshotHandle m_hLocal;
        public EResult m_eResult;
    }
}

