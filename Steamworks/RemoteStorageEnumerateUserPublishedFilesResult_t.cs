namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x520)]
    public struct RemoteStorageEnumerateUserPublishedFilesResult_t
    {
        public const int k_iCallback = 0x520;
        public EResult m_eResult;
        public int m_nResultsReturned;
        public int m_nTotalResultCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public PublishedFileId_t[] m_rgPublishedFileId;
    }
}

