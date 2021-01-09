namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(-4247034853035810941L)]
    public class CriticalDamageEvent : Event
    {
        public Entity Target { get; set; }

        public Vector3 LocalPosition { get; set; }
    }
}

