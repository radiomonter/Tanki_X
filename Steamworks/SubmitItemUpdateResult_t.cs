namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd4c)]
    public struct SubmitItemUpdateResult_t
    {
        public const int k_iCallback = 0xd4c;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
    }
}

