namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd4e)]
    public struct DownloadItemResult_t
    {
        public const int k_iCallback = 0xd4e;
        public AppId_t m_unAppID;
        public PublishedFileId_t m_nPublishedFileId;
        public EResult m_eResult;
    }
}

