namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x15f28f3dfceL)]
    public class AutopilotWeaponControllerComponent : SharedChangeableComponent
    {
        public bool Attack { get; set; }

        public bool TragerAchievable { get; set; }

        [ProtocolOptional]
        public Entity Target { get; set; }

        public float Accurasy { get; set; }

        [ProtocolTransient]
        public bool Fire { get; set; }

        [ProtocolTransient]
        public bool ShouldMiss { get; set; }

        [ProtocolTransient]
        public Rigidbody TargetRigidbody { get; set; }

        [ProtocolTransient]
        public bool IsOnShootingLine { get; set; }
    }
}

