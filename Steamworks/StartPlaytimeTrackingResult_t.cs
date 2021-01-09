namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd52)]
    public struct StartPlaytimeTrackingResult_t
    {
        public const int k_iCallback = 0xd52;
        public EResult m_eResult;
    }
}

