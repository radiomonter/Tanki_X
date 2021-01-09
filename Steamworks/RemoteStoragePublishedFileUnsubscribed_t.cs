namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x52a)]
    public struct RemoteStoragePublishedFileUnsubscribed_t
    {
        public const int k_iCallback = 0x52a;
        public PublishedFileId_t m_nPublishedFileId;
        public AppId_t m_nAppID;
    }
}

