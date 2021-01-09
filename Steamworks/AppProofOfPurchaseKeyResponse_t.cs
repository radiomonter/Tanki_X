namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x3fd)]
    public struct AppProofOfPurchaseKeyResponse_t
    {
        public const int k_iCallback = 0x3fd;
        public EResult m_eResult;
        public uint m_nAppID;
        public uint m_cchKeyLength;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=240)]
        public string m_rgchKey;
    }
}

