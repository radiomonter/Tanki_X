namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x15a)]
    public struct FriendsEnumerateFollowingList_t
    {
        public const int k_iCallback = 0x15a;
        public EResult m_eResult;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=50)]
        public CSteamID[] m_rgSteamID;
        public int m_nResultsReturned;
        public int m_nTotalResultCount;
    }
}

