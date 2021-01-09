namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x2c1)]
    public struct CheckFileSignature_t
    {
        public const int k_iCallback = 0x2c1;
        public ECheckFileSignature m_eCheckFileSignature;
    }
}

