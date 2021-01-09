namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xa5)]
    public struct StoreAuthURLResponse_t
    {
        public const int k_iCallback = 0xa5;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x200)]
        public string m_szURL;
    }
}

