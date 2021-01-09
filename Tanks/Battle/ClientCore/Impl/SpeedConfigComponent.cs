﻿namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-177474741853856725L)]
    public class SpeedConfigComponent : Component
    {
        public float ReverseAcceleration { get; set; }

        public float SideAcceleration { get; set; }

        public float TurnAcceleration { get; set; }

        public float ReverseTurnAcceleration { get; set; }
    }
}

