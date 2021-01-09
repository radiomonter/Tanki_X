namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WeaponRotationUpdateInputEvent : Event
    {
        public WeaponRotationUpdateInputEvent(float deltaTime)
        {
            this.DeltaTime = deltaTime;
        }

        public float DeltaTime { get; set; }
    }
}

