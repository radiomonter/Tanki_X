namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x528)]
    public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
    {
        public const int k_iCallback = 0x528;
        public EResult m_eResult;
        public PublishedFileId_t m_unPublishedFileId;
        public int m_nVotesFor;
        public int m_nVotesAgainst;
        public int m_nReports;
        public float m_fScore;
    }
}

