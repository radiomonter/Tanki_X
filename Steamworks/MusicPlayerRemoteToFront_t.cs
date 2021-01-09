namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1, Pack=8), CallbackIdentity(0x1007)]
    public struct MusicPlayerRemoteToFront_t
    {
        public const int k_iCallback = 0x1007;
    }
}

