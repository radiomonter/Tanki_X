namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x204)]
    public struct FavoritesListAccountsUpdated_t
    {
        public const int k_iCallback = 0x204;
        public EResult m_eResult;
    }
}

