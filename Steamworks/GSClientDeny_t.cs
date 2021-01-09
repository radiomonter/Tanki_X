namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0xca)]
    public struct GSClientDeny_t
    {
        public const int k_iCallback = 0xca;
        public CSteamID m_SteamID;
        public EDenyReason m_eDenyReason;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
        public string m_rgchOptionalText;
    }
}

