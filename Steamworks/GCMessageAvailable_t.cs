namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x6a5)]
    public struct GCMessageAvailable_t
    {
        public const int k_iCallback = 0x6a5;
        public uint m_nMessageSize;
    }
}

