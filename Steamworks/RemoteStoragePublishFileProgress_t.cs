namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x531)]
    public struct RemoteStoragePublishFileProgress_t
    {
        public const int k_iCallback = 0x531;
        public double m_dPercentFile;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bPreview;
    }
}

