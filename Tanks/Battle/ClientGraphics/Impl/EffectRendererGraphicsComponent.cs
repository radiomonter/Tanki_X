namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EffectRendererGraphicsComponent : Component
    {
        public EffectRendererGraphicsComponent()
        {
        }

        public EffectRendererGraphicsComponent(UnityEngine.Renderer renderer)
        {
            this.Renderer = renderer;
        }

        public UnityEngine.Renderer Renderer { get; set; }
    }
}

