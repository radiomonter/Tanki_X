namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SpawnPointComponent : Component
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
    }
}

