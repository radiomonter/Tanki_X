namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x14c)]
    public struct GameServerChangeRequested_t
    {
        public const int k_iCallback = 0x14c;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
        public string m_rgchServer;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
        public string m_rgchPassword;
    }
}

