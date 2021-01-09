namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankVisualAssemblySystem : ECSSystem
    {
        [OnEventFire]
        public void ContinueAssebleTank(ContinueAssembleTankEvent e, TankGraphicsNode tank)
        {
            tank.Entity.AddComponent<AssembledTankInactiveStateComponent>();
        }

        private void InitCharacterShadowSystem(Entity tankEntity, Transform tankVisualRoot, Transform weaponVisualRoot)
        {
            CharacterShadowCastersComponent component2 = new CharacterShadowCastersComponent();
            component2.Casters = new Transform[] { tankVisualRoot, weaponVisualRoot };
            CharacterShadowCastersComponent component = component2;
            tankEntity.AddComponent(component);
        }

        [OnEventFire]
        public void LocateWeaponVisualRootUnderMountPoint(NodeAddedEvent evt, TankGraphicsNode tank, [Context, JoinByTank] WeaponGraphicsNode weaponGraphics)
        {
            WeaponVisualRootComponent weaponVisualRoot = weaponGraphics.weaponVisualRoot;
            weaponVisualRoot.transform.parent = tank.tankVisualRoot.transform;
            Transform mountPoint = tank.mountPoint.MountPoint;
            GameObject obj2 = new GameObject("VisualMountPoint");
            obj2.transform.SetParent(tank.tankVisualRoot.transform, false);
            obj2.transform.localPosition = mountPoint.localPosition;
            obj2.transform.localRotation = mountPoint.localRotation;
            VisualMountPointComponent component = new VisualMountPointComponent {
                MountPoint = obj2.transform
            };
            tank.Entity.AddComponent(component);
            weaponVisualRoot.transform.SetParent(obj2.transform, false);
            weaponVisualRoot.transform.localPosition = Vector3.zero;
            weaponVisualRoot.transform.localRotation = Quaternion.identity;
            this.InitCharacterShadowSystem(tank.Entity, tank.tankVisualRoot.transform, weaponGraphics.weaponVisualRoot.transform);
            base.NewEvent<ContinueAssembleTankEvent>().Attach(tank).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void OnTankPartsPrepared(NodeAddedEvent e, AssembledTankNode tank, [JoinAll] SingleNode<CameraRootTransformComponent> cameraNode)
        {
            tank.tankVisualRoot.transform.parent = tank.assembledTank.AssemblyRoot.transform;
            CameraVisibleTriggerComponent component = tank.trackRenderer.Renderer.gameObject.AddComponent<CameraVisibleTriggerComponent>();
            tank.Entity.AddComponent(component);
            component.MainCameraTransform = cameraNode.component.Root;
        }

        public class AssembledTankNode : Node
        {
            public AssembledTankComponent assembledTank;
            public TankVisualRootComponent tankVisualRoot;
            public TrackRendererComponent trackRenderer;
        }

        public class TankGraphicsNode : Node
        {
            public AssembledTankComponent assembledTank;
            public TankGroupComponent tankGroup;
            public MountPointComponent mountPoint;
            public TankVisualRootComponent tankVisualRoot;
        }

        public class WeaponGraphicsNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}

