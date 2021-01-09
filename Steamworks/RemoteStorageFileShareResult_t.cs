namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x51b)]
    public struct RemoteStorageFileShareResult_t
    {
        public const int k_iCallback = 0x51b;
        public EResult m_eResult;
        public UGCHandle_t m_hFile;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string m_rgchFilename;
    }
}

