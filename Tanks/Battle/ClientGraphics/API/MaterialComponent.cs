namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MaterialComponent : Component
    {
        public MaterialComponent(UnityEngine.Material material)
        {
            this.Material = material;
        }

        public UnityEngine.Material Material { get; set; }
    }
}

