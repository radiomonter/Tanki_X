namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x125f)]
    public struct SteamInventoryEligiblePromoItemDefIDs_t
    {
        public const int k_iCallback = 0x125f;
        public EResult m_result;
        public CSteamID m_steamID;
        public int m_numEligiblePromoItemDefs;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bCachedData;
    }
}

