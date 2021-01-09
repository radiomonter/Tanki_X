namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class InitMinePlacingTransformEvent : Event
    {
        public InitMinePlacingTransformEvent(Vector3 position)
        {
            this.Position = position;
        }

        public Vector3 Position { get; set; }
    }
}

