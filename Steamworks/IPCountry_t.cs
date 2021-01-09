namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1, Pack=8), CallbackIdentity(0x2bd)]
    public struct IPCountry_t
    {
        public const int k_iCallback = 0x2bd;
    }
}

