namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x2be)]
    public struct LowBatteryPower_t
    {
        public const int k_iCallback = 0x2be;
        public byte m_nMinutesBatteryLeft;
    }
}

