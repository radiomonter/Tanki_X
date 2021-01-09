namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x525)]
    public struct RemoteStorageDownloadUGCResult_t
    {
        public const int k_iCallback = 0x525;
        public EResult m_eResult;
        public UGCHandle_t m_hFile;
        public AppId_t m_nAppID;
        public int m_nSizeInBytes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string m_pchFileName;
        public ulong m_ulSteamIDOwner;
    }
}

