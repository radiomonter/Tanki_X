namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class DroneFlySoundEffectSystem : ECSSystem
    {
        private const float MIN_VELOCITY = 0.1f;

        [OnEventFire]
        public void PlayDroneFlySound(NodeAddedEvent e, DroneEffectNode drone)
        {
            drone.Entity.AddComponent<DroneFlySoundEffectReadyComponent>();
            drone.droneFlySoundEffect.Sound.FadeIn();
        }

        [OnEventFire]
        public void RemoveEffect(RemoveEffectEvent e, ReadyDroneEffectNode drone)
        {
            drone.Entity.RemoveComponent<DroneFlySoundEffectReadyComponent>();
            drone.droneFlySoundEffect.Sound.FadeOut();
        }

        public class DroneEffectNode : Node
        {
            public DroneEffectComponent droneEffect;
            public DroneFlySoundEffectComponent droneFlySoundEffect;
            public RigidbodyComponent rigidbody;
        }

        public class ReadyDroneEffectNode : DroneFlySoundEffectSystem.DroneEffectNode
        {
            public DroneFlySoundEffectReadyComponent droneFlySoundEffectReady;
        }
    }
}

