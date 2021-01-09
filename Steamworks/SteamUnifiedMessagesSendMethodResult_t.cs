namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x9c5)]
    public struct SteamUnifiedMessagesSendMethodResult_t
    {
        public const int k_iCallback = 0x9c5;
        public ClientUnifiedMessageHandle m_hHandle;
        public ulong m_unContext;
        public EResult m_eResult;
        public uint m_unResponseSize;
    }
}

