namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x52d)]
    public struct RemoteStorageUserVoteDetails_t
    {
        public const int k_iCallback = 0x52d;
        public EResult m_eResult;
        public PublishedFileId_t m_nPublishedFileId;
        public EWorkshopVote m_eVote;
    }
}

