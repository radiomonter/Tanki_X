namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x516)]
    public struct RemoteStorageAppSyncedServer_t
    {
        public const int k_iCallback = 0x516;
        public AppId_t m_nAppID;
        public EResult m_eResult;
        public int m_unNumUploads;
    }
}

