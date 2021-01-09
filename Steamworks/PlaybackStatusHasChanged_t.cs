namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1, Pack=8), CallbackIdentity(0xfa1)]
    public struct PlaybackStatusHasChanged_t
    {
        public const int k_iCallback = 0xfa1;
    }
}

