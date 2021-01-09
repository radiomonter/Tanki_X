namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x3ff)]
    public struct FileDetailsResult_t
    {
        public const int k_iCallback = 0x3ff;
        public EResult m_eResult;
        public ulong m_ulFileSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=20)]
        public byte[] m_FileSHA;
        public uint m_unFlags;
    }
}

