namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x98)]
    public struct MicroTxnAuthorizationResponse_t
    {
        public const int k_iCallback = 0x98;
        public uint m_unAppID;
        public ulong m_ulOrderID;
        public byte m_bAuthorized;
    }
}

