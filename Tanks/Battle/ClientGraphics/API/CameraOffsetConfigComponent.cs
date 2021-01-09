namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CameraOffsetConfigComponent : Component
    {
        public float XOffset { get; set; }

        public float YOffset { get; set; }

        public float ZOffset { get; set; }
    }
}

