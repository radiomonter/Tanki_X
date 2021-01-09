namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xfac)]
    public struct MusicPlayerSelectsQueueEntry_t
    {
        public const int k_iCallback = 0xfac;
        public int nID;
    }
}

