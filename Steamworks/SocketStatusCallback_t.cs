namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x4b1)]
    public struct SocketStatusCallback_t
    {
        public const int k_iCallback = 0x4b1;
        public SNetSocket_t m_hSocket;
        public SNetListenSocket_t m_hListenSocket;
        public CSteamID m_steamIDRemote;
        public int m_eSNetSocketState;
    }
}

