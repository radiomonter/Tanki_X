namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CharacterShadowInternalComponent : Component
    {
        public UnityEngine.Projector Projector { get; set; }

        public Material CasterMaterial { get; set; }

        public float BaseAlpha { get; set; }

        public Bounds ProjectionBoundInLightSpace { get; set; }
    }
}

