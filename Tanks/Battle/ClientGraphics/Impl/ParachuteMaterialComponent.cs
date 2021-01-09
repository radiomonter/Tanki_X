namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ParachuteMaterialComponent : Component
    {
        public ParachuteMaterialComponent(UnityEngine.Material material)
        {
            this.Material = material;
        }

        public UnityEngine.Material Material { get; set; }
    }
}

