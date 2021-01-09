namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShaftAimingRendererReducingAlphaComponent : Component
    {
        public ShaftAimingRendererReducingAlphaComponent()
        {
        }

        public ShaftAimingRendererReducingAlphaComponent(float initialAlpha)
        {
            this.InitialAlpha = initialAlpha;
        }

        public float InitialAlpha { get; set; }
    }
}

