namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x15b)]
    public struct SetPersonaNameResponse_t
    {
        public const int k_iCallback = 0x15b;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bSuccess;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bLocalSuccess;
        public EResult m_result;
    }
}

