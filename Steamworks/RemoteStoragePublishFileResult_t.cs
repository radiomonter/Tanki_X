namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x51d)]
    public struct RemoteStoragePublishFileResult_t
    {
        public const int k_iCallback = 0x51d;
        public EResult m_eResult;
        public PublishedFileId_t m_nPublishedFileId;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
    }
}

