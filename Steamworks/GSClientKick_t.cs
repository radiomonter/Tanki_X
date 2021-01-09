namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0xcb)]
    public struct GSClientKick_t
    {
        public const int k_iCallback = 0xcb;
        public CSteamID m_SteamID;
        public EDenyReason m_eDenyReason;
    }
}

