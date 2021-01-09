namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2f408c536ae9bL)]
    public class HangarInstanceComponent : Component
    {
        public HangarInstanceComponent()
        {
        }

        public HangarInstanceComponent(GameObject sceneRoot)
        {
            this.SceneRoot = sceneRoot;
        }

        public GameObject SceneRoot { get; set; }
    }
}

