namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1203)]
    public struct GetVideoURLResult_t
    {
        public const int k_iCallback = 0x1203;
        public EResult m_eResult;
        public AppId_t m_unVideoAppID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_rgchURL;
    }
}

