namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), CallbackIdentity(0x14e)]
    public struct AvatarImageLoaded_t
    {
        public const int k_iCallback = 0x14e;
        public CSteamID m_steamID;
        public int m_iImage;
        public int m_iWide;
        public int m_iTall;
    }
}

