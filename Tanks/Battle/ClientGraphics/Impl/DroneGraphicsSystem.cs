namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;

    public class DroneGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void InitDroneWeapon(NodeAddedEvent e, [Combine] DroneWeaponNode droneWeapon, [JoinByUnit, Context] DroneEffectNode droneEffect, SingleNode<CameraRootTransformComponent> camera)
        {
            CameraVisibleTriggerComponent component = droneEffect.effectRendererGraphics.Renderer.gameObject.AddComponent<CameraVisibleTriggerComponent>();
            component.MainCameraTransform = camera.component.Root;
            if (!droneWeapon.Entity.HasComponent<CameraVisibleTriggerComponent>())
            {
                droneWeapon.Entity.AddComponent(component);
            }
        }

        public class DroneEffectNode : Node
        {
            public DroneEffectComponent droneEffect;
            public EffectRendererGraphicsComponent effectRendererGraphics;
            public RigidbodyComponent rigidbody;
            public UnitGroupComponent unitGroup;
        }

        public class DroneWeaponNode : Node
        {
            public DroneWeaponComponent droneWeapon;
            public WeaponVisualRootComponent weaponVisualRoot;
            public UnitGroupComponent unitGroup;
        }
    }
}

