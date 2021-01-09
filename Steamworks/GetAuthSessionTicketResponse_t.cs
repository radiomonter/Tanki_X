namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xa3)]
    public struct GetAuthSessionTicketResponse_t
    {
        public const int k_iCallback = 0xa3;
        public HAuthTicket m_hAuthTicket;
        public EResult m_eResult;
    }
}

