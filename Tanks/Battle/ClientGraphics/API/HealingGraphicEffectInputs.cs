namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class HealingGraphicEffectInputs
    {
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;
        private SkinnedMeshRenderer renderer;

        public HealingGraphicEffectInputs(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity, SkinnedMeshRenderer renderer)
        {
            this.entity = entity;
            this.renderer = renderer;
        }

        public SkinnedMeshRenderer Renderer =>
            this.renderer;

        public virtual float TilingX =>
            4f;

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity =>
            this.entity;
    }
}

