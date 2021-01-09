namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x155)]
    public struct DownloadClanActivityCountsResult_t
    {
        public const int k_iCallback = 0x155;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bSuccess;
    }
}

