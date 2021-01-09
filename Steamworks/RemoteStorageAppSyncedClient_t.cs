namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x515)]
    public struct RemoteStorageAppSyncedClient_t
    {
        public const int k_iCallback = 0x515;
        public AppId_t m_nAppID;
        public EResult m_eResult;
        public int m_unNumDownloads;
    }
}

