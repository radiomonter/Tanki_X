namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xc9)]
    public struct GSClientApprove_t
    {
        public const int k_iCallback = 0xc9;
        public CSteamID m_SteamID;
        public CSteamID m_OwnerSteamID;
    }
}

