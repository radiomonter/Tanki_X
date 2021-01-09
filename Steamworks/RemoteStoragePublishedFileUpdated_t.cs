namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x532)]
    public struct RemoteStoragePublishedFileUpdated_t
    {
        public const int k_iCallback = 0x532;
        public PublishedFileId_t m_nPublishedFileId;
        public AppId_t m_nAppID;
        public ulong m_ulUnused;
    }
}

