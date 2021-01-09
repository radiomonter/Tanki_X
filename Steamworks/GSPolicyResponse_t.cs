namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x73)]
    public struct GSPolicyResponse_t
    {
        public const int k_iCallback = 0x73;
        public byte m_bSecure;
    }
}

