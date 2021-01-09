namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x151)]
    public struct GameRichPresenceJoinRequested_t
    {
        public const int k_iCallback = 0x151;
        public CSteamID m_steamIDFriend;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_rgchConnect;
    }
}

