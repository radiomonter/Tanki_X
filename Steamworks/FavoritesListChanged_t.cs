﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1f6)]
    public struct FavoritesListChanged_t
    {
        public const int k_iCallback = 0x1f6;
        public uint m_nIP;
        public uint m_nQueryPort;
        public uint m_nConnPort;
        public uint m_nAppID;
        public uint m_nFlags;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bAdd;
        public AccountID_t m_unAccountId;
    }
}

