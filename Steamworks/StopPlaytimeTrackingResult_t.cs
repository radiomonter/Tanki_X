namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd53)]
    public struct StopPlaytimeTrackingResult_t
    {
        public const int k_iCallback = 0xd53;
        public EResult m_eResult;
    }
}

