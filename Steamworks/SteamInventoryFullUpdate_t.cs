namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x125d)]
    public struct SteamInventoryFullUpdate_t
    {
        public const int k_iCallback = 0x125d;
        public SteamInventoryResult_t m_handle;
    }
}

