namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftAimingStraightTargetingEvent : Event
    {
        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }

        public Vector3 WorkingDirection { get; set; }
    }
}

