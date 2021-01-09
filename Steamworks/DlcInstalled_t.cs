namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x3ed)]
    public struct DlcInstalled_t
    {
        public const int k_iCallback = 0x3ed;
        public AppId_t m_nAppID;
    }
}

