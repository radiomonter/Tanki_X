namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x14f)]
    public struct ClanOfficerListResponse_t
    {
        public const int k_iCallback = 0x14f;
        public CSteamID m_steamIDClan;
        public int m_cOfficers;
        public byte m_bSuccess;
    }
}

