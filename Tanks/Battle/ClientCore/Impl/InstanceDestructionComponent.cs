namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e1592e909aL)]
    public class InstanceDestructionComponent : Component
    {
        public InstanceDestructionComponent()
        {
        }

        public InstanceDestructionComponent(UnityEngine.GameObject gameObject)
        {
            this.GameObject = gameObject;
        }

        public UnityEngine.GameObject GameObject { get; set; }
    }
}

