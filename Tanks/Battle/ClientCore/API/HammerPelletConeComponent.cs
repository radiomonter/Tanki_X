namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x155162a4340L)]
    public class HammerPelletConeComponent : Component
    {
        public float HorizontalConeHalfAngle { get; set; }

        public float VerticalConeHalfAngle { get; set; }

        public int PelletCount { get; set; }

        [ProtocolTransient]
        public int ShotSeed { get; set; }
    }
}

