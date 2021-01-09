namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UpgradeCoefs
    {
        public float tankCoeff;
        public float weaponCoeff;
        public float tankCoeffWithSelected;
        public float weaponCoeffWithSelected;
    }
}

