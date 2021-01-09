namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x52c)]
    public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
    {
        public const int k_iCallback = 0x52c;
        public EResult m_eResult;
        public PublishedFileId_t m_nPublishedFileId;
    }
}

