namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x8d3e24f113efc9cL)]
    public class GraffitiDecalComponent : Component
    {
        public GraffitiDecalComponent()
        {
        }

        public GraffitiDecalComponent(Vector3 sprayPosition, Vector3 sprayDirection, Vector3 sprayUpDirection)
        {
            this.SprayPosition = sprayPosition;
            this.SprayDirection = sprayDirection;
            this.SprayUpDirection = sprayUpDirection;
        }

        public Vector3 SprayPosition { get; set; }

        public Vector3 SprayDirection { get; set; }

        public Vector3 SprayUpDirection { get; set; }
    }
}

