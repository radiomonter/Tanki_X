namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x753581181d1ff3f9L)]
    public class ShaftAimingTargetPointComponent : SharedChangeableComponent
    {
        [ProtocolOptional]
        public Vector3 Point { get; set; }

        public bool IsInsideTankPart { get; set; }
    }
}

