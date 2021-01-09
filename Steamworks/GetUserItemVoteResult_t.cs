namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd51)]
    public struct GetUserItemVoteResult_t
    {
        public const int k_iCallback = 0xd51;
        public PublishedFileId_t m_nPublishedFileId;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bVotedUp;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bVotedDown;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bVoteSkipped;
    }
}

