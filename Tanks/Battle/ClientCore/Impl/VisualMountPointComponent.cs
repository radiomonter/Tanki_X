namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class VisualMountPointComponent : Component
    {
        public Transform MountPoint { get; set; }
    }
}

