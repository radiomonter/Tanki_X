namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x517)]
    public struct RemoteStorageAppSyncProgress_t
    {
        public const int k_iCallback = 0x517;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string m_rgchCurrentFile;
        public AppId_t m_nAppID;
        public uint m_uBytesTransferredThisChunk;
        public double m_dAppPercentComplete;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bUploading;
    }
}

