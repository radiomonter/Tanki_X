namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x125c)]
    public struct SteamInventoryResultReady_t
    {
        public const int k_iCallback = 0x125c;
        public SteamInventoryResult_t m_handle;
        public EResult m_result;
    }
}

