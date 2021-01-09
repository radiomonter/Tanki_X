namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GraffitiInstanceComponent : Component
    {
        public GraffitiInstanceComponent()
        {
        }

        public GraffitiInstanceComponent(GameObject graffitiInstance)
        {
            this.GraffitiInstance = graffitiInstance;
        }

        public GameObject GraffitiInstance { get; set; }

        public GameObject GraffitiDecalObject { get; set; }

        public Renderer EmitRenderer { get; set; }
    }
}

