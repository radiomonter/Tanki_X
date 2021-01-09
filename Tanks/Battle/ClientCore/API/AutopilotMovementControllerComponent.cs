namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x15f28e8c950L)]
    public class AutopilotMovementControllerComponent : Component
    {
        public bool Moving { get; set; }

        public bool MoveToTarget { get; set; }

        [ProtocolOptional]
        public Entity Target { get; set; }

        public Vector3 Destination { get; set; }
    }
}

