namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11fd)]
    public struct BroadcastUploadStop_t
    {
        public const int k_iCallback = 0x11fd;
        public EBroadcastUploadResult m_eResult;
    }
}

