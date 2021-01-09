namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e13b79206aL)]
    public class BulletComponent : Component
    {
        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public float Distance { get; set; }

        public float LastUpdateTime { get; set; }

        public float Radius { get; set; }

        public float Speed { get; set; }

        public int ShotId { get; set; }
    }
}

