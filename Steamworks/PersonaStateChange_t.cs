namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x130)]
    public struct PersonaStateChange_t
    {
        public const int k_iCallback = 0x130;
        public ulong m_ulSteamID;
        public EPersonaChange m_nChangeFlags;
    }
}

