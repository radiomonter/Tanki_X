namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x15c1103ffa2L)]
    public class DamageInfoEvent : Event
    {
        public float Damage { get; set; }

        public Vector3 HitPoint { get; set; }

        public bool BackHit { get; set; }

        public bool HealHit { get; set; }
    }
}

