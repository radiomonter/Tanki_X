namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x4b2)]
    public struct P2PSessionRequest_t
    {
        public const int k_iCallback = 0x4b2;
        public CSteamID m_steamIDRemote;
    }
}

