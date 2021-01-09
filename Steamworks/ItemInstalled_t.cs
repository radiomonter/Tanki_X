namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd4d)]
    public struct ItemInstalled_t
    {
        public const int k_iCallback = 0xd4d;
        public AppId_t m_unAppID;
        public PublishedFileId_t m_nPublishedFileId;
    }
}

