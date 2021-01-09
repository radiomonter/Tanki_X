namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using UnityEngine;

    public static class HardwareFingerprintUtils
    {
        private static string hardwareFingerprint;

        public static string HardwareFingerprint
        {
            get => 
                hardwareFingerprint ?? SystemInfo.deviceUniqueIdentifier;
            set => 
                hardwareFingerprint = value;
        }
    }
}

