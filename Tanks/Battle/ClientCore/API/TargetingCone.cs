namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct TargetingCone
    {
        public float HAngle { get; set; }
        public float VAngleUp { get; set; }
        public float VAngleDown { get; set; }
        public float Distance { get; set; }
    }
}

