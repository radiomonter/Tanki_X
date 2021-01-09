namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct ModuleUpgradeCharacteristic
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Current { get; set; }
        public float Next { get; set; }
        public string CurrentStr { get; set; }
        public string NextStr { get; set; }
    }
}

