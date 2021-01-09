namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x527)]
    public struct RemoteStorageEnumerateWorkshopFilesResult_t
    {
        public const int k_iCallback = 0x527;
        public EResult m_eResult;
        public int m_nResultsReturned;
        public int m_nTotalResultCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public PublishedFileId_t[] m_rgPublishedFileId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public float[] m_rgScore;
        public AppId_t m_nAppId;
        public uint m_unStartIndex;
    }
}

