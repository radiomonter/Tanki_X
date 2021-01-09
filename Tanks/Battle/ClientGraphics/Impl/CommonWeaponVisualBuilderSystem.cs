namespace Tanks.Battle.ClientGraphics.Impl
{
    using CurvedUI;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class CommonWeaponVisualBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildVisualWeapon(NodeAddedEvent evt, WeaponGraphicsNode weaponGraphics)
        {
            Entity entity = weaponGraphics.Entity;
            BaseRendererComponent component = new BaseRendererComponent();
            Renderer weaponRenderer = TankBuilderUtil.GetWeaponRenderer(weaponGraphics.weaponVisualRoot.gameObject);
            component.Renderer = weaponRenderer;
            component.Mesh = (component.Renderer as SkinnedMeshRenderer).sharedMesh;
            entity.AddComponent<StartMaterialsComponent>();
            entity.AddComponent(component);
            WeaponBoundsComponent component3 = new WeaponBoundsComponent {
                WeaponBounds = weaponRenderer.bounds
            };
            entity.AddComponent(component3);
        }

        [OnEventFire]
        public void BuildWeaponGraphics(NodeAddedEvent e, [Combine] WeaponGraphicsRendererNode weapon, BattleCameraNode camera)
        {
            Renderer renderer = weapon.baseRenderer.Renderer;
            if (weapon.Entity.HasComponent<CameraVisibleTriggerComponent>())
            {
                weapon.Entity.GetComponent<CameraVisibleTriggerComponent>().MainCameraTransform = camera.cameraRootTransform.Root;
            }
            else
            {
                CameraVisibleTriggerComponent component = renderer.gameObject.AddComponentIfMissing<CameraVisibleTriggerComponent>();
                component.MainCameraTransform = camera.cameraRootTransform.Root;
                weapon.Entity.AddComponent(component);
            }
        }

        public class BattleCameraNode : Node
        {
            public CameraRootTransformComponent cameraRootTransform;
            public BattleCameraComponent battleCamera;
        }

        public class WeaponGraphicsNode : Node
        {
            public WeaponVisualRootComponent weaponVisualRoot;
            public TankGroupComponent tankGroup;
        }

        public class WeaponGraphicsRendererNode : CommonWeaponVisualBuilderSystem.WeaponGraphicsNode
        {
            public BaseRendererComponent baseRenderer;
            public WeaponPreparedByEntityBehaviourComponent weaponPreparedByEntityBehaviour;
        }
    }
}

