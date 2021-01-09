namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd50)]
    public struct SetUserItemVoteResult_t
    {
        public const int k_iCallback = 0xd50;
        public PublishedFileId_t m_nPublishedFileId;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bVoteUp;
    }
}

