namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x526)]
    public struct RemoteStorageGetPublishedFileDetailsResult_t
    {
        public const int k_iCallback = 0x526;
        public EResult m_eResult;
        public PublishedFileId_t m_nPublishedFileId;
        public AppId_t m_nCreatorAppID;
        public AppId_t m_nConsumerAppID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x81)]
        public string m_rgchTitle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x1f40)]
        public string m_rgchDescription;
        public UGCHandle_t m_hFile;
        public UGCHandle_t m_hPreviewFile;
        public ulong m_ulSteamIDOwner;
        public uint m_rtimeCreated;
        public uint m_rtimeUpdated;
        public ERemoteStoragePublishedFileVisibility m_eVisibility;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bBanned;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x401)]
        public string m_rgchTags;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bTagsTruncated;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string m_pchFileName;
        public int m_nFileSize;
        public int m_nPreviewFileSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_rgchURL;
        public EWorkshopFileType m_eFileType;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bAcceptedForUse;
    }
}

