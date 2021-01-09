namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xa4)]
    public struct GameWebCallback_t
    {
        public const int k_iCallback = 0xa4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_szURL;
    }
}

