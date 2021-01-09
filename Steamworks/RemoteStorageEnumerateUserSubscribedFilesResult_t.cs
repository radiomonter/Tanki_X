namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x522)]
    public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
    {
        public const int k_iCallback = 0x522;
        public EResult m_eResult;
        public int m_nResultsReturned;
        public int m_nTotalResultCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public PublishedFileId_t[] m_rgPublishedFileId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public uint[] m_rgRTimeSubscribed;
    }
}

