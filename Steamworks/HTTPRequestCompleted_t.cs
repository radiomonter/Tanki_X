﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x835)]
    public struct HTTPRequestCompleted_t
    {
        public const int k_iCallback = 0x835;
        public HTTPRequestHandle m_hRequest;
        public ulong m_ulContextValue;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bRequestSuccessful;
        public EHTTPStatusCode m_eStatusCode;
        public uint m_unBodySize;
    }
}

