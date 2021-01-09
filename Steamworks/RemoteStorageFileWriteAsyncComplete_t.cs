namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x533)]
    public struct RemoteStorageFileWriteAsyncComplete_t
    {
        public const int k_iCallback = 0x533;
        public EResult m_eResult;
    }
}

