namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x66)]
    public struct SteamServerConnectFailure_t
    {
        public const int k_iCallback = 0x66;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bStillRetrying;
    }
}

