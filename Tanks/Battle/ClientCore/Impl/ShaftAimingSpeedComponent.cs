namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-7212768015824297898L)]
    public class ShaftAimingSpeedComponent : Component
    {
        public float MaxVerticalSpeed { get; set; }

        public float MaxHorizontalSpeed { get; set; }

        public float HorizontalAcceleration { get; set; }

        public float VerticalAcceleration { get; set; }
    }
}

