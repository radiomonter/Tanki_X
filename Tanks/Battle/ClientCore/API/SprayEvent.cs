namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SprayEvent : Event
    {
        public SprayEvent()
        {
        }

        public SprayEvent(Vector3 position)
        {
            this.Position = position;
        }

        public override string ToString() => 
            $"Position: {this.Position}";

        public Vector3 Position { get; set; }
    }
}

