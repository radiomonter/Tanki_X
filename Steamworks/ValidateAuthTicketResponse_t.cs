namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x8f)]
    public struct ValidateAuthTicketResponse_t
    {
        public const int k_iCallback = 0x8f;
        public CSteamID m_SteamID;
        public EAuthSessionResponse m_eAuthSessionResponse;
        public CSteamID m_OwnerSteamID;
    }
}

