﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xf3d)]
    public struct SteamAppInstalled_t
    {
        public const int k_iCallback = 0xf3d;
        public AppId_t m_nAppID;
    }
}

