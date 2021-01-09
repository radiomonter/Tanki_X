namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x52b)]
    public struct RemoteStoragePublishedFileDeleted_t
    {
        public const int k_iCallback = 0x52b;
        public PublishedFileId_t m_nPublishedFileId;
        public AppId_t m_nAppID;
    }
}

