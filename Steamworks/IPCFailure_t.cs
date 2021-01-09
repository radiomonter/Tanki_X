namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x75)]
    public struct IPCFailure_t
    {
        public const int k_iCallback = 0x75;
        public byte m_eFailureType;
    }
}

