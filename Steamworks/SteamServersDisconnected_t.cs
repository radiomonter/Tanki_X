namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x67)]
    public struct SteamServersDisconnected_t
    {
        public const int k_iCallback = 0x67;
        public EResult m_eResult;
    }
}

