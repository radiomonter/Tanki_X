namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BaseRendererComponent : Component
    {
        public UnityEngine.Renderer Renderer { get; set; }

        public UnityEngine.Mesh Mesh { get; set; }
    }
}

