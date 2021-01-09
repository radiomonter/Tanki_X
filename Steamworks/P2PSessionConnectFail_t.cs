namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1), CallbackIdentity(0x4b3)]
    public struct P2PSessionConnectFail_t
    {
        public const int k_iCallback = 0x4b3;
        public CSteamID m_steamIDRemote;
        public byte m_eP2PSessionError;
    }
}

