namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd4f)]
    public struct UserFavoriteItemsListChanged_t
    {
        public const int k_iCallback = 0xd4f;
        public PublishedFileId_t m_nPublishedFileId;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bWasAddRequest;
    }
}

