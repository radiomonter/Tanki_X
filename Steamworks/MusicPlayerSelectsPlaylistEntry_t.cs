namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xfad)]
    public struct MusicPlayerSelectsPlaylistEntry_t
    {
        public const int k_iCallback = 0xfad;
        public int nID;
    }
}

