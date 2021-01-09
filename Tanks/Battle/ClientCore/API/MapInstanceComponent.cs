namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e14108c58aL)]
    public class MapInstanceComponent : Component
    {
        public MapInstanceComponent()
        {
        }

        public MapInstanceComponent(GameObject sceneRoot)
        {
            this.SceneRoot = sceneRoot;
        }

        public GameObject SceneRoot { get; set; }
    }
}

