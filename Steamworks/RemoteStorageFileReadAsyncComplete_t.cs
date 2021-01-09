namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x534)]
    public struct RemoteStorageFileReadAsyncComplete_t
    {
        public const int k_iCallback = 0x534;
        public SteamAPICall_t m_hFileReadAsync;
        public EResult m_eResult;
        public uint m_nOffset;
        public uint m_cubRead;
    }
}

