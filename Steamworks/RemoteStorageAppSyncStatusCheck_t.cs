namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x519)]
    public struct RemoteStorageAppSyncStatusCheck_t
    {
        public const int k_iCallback = 0x519;
        public AppId_t m_nAppID;
        public EResult m_eResult;
    }
}

