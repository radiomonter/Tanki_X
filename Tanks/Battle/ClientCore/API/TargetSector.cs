namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct TargetSector
    {
        public float Down { get; set; }
        public float Up { get; set; }
        public float Distance { get; set; }
        public float Length() => 
            this.Up - this.Down;
    }
}

