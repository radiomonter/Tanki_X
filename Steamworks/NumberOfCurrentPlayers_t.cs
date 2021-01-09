namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x453)]
    public struct NumberOfCurrentPlayers_t
    {
        public const int k_iCallback = 0x453;
        public byte m_bSuccess;
        public int m_cPlayers;
    }
}

