namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x9a)]
    public struct EncryptedAppTicketResponse_t
    {
        public const int k_iCallback = 0x9a;
        public EResult m_eResult;
    }
}

