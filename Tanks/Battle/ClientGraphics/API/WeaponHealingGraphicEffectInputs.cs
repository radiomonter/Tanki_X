namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class WeaponHealingGraphicEffectInputs : HealingGraphicEffectInputs
    {
        private Transform rotationTransform;

        public WeaponHealingGraphicEffectInputs(Entity entity, Transform rotationTransform, SkinnedMeshRenderer renderer) : base(entity, renderer)
        {
            this.rotationTransform = rotationTransform;
        }

        public Transform RotationTransform =>
            this.rotationTransform;

        public override float TilingX =>
            2f;
    }
}

