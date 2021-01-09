namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x3fe9b7cdc343483bL)]
    public class PositionComponent : Component
    {
        public PositionComponent()
        {
        }

        public PositionComponent(Vector3 position)
        {
            this.Position = position;
        }

        public Vector3 Position { get; set; }
    }
}

