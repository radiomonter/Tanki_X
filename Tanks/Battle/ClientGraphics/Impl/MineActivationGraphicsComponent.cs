namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MineActivationGraphicsComponent : Component
    {
        public MineActivationGraphicsComponent()
        {
        }

        public MineActivationGraphicsComponent(float activationStartTime)
        {
            this.ActivationStartTime = activationStartTime;
        }

        public float ActivationStartTime { get; set; }
    }
}

