namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MaterialArrayComponent : Component
    {
        public MaterialArrayComponent(Material[] materials)
        {
            this.Materials = materials;
        }

        public Material[] Materials { get; set; }
    }
}

