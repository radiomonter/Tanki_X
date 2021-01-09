namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1), CallbackIdentity(0xd0)]
    public struct GSClientGroupStatus_t
    {
        public const int k_iCallback = 0xd0;
        public CSteamID m_SteamIDUser;
        public CSteamID m_SteamIDGroup;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bMember;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bOfficer;
    }
}

