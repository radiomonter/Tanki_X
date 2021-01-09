namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x3f0)]
    public struct RegisterActivationCodeResponse_t
    {
        public const int k_iCallback = 0x3f0;
        public ERegisterActivationCodeResult m_eResult;
        public uint m_unPackageRegistered;
    }
}

