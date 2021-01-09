namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x44e)]
    public struct UserStatsStored_t
    {
        public const int k_iCallback = 0x44e;
        public ulong m_nGameID;
        public EResult m_eResult;
    }
}

