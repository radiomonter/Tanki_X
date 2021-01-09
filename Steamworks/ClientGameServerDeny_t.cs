namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x71)]
    public struct ClientGameServerDeny_t
    {
        public const int k_iCallback = 0x71;
        public uint m_uAppID;
        public uint m_unGameServerIP;
        public ushort m_usGameServerPort;
        public ushort m_bSecure;
        public uint m_uReason;
    }
}

