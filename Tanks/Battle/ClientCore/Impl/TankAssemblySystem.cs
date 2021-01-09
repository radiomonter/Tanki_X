namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankAssemblySystem : ECSSystem
    {
        [OnEventComplete]
        public void ActivateAssembledTank(NodeAddedEvent e, AssembledActivationTankNode tank, [JoinByTank] WeaponNode weapon)
        {
            tank.assembledTank.AssemblyRoot.SetActive(true);
            tank.hullInstance.HullInstance.SetActive(true);
            weapon.weaponInstance.WeaponInstance.SetActive(true);
            tank.Entity.RemoveComponent<AssembledTankInactiveStateComponent>();
            tank.Entity.AddComponent<AssembledTankActivatedStateComponent>();
        }

        [OnEventFire]
        public void AddTargetBehaviour(NodeAddedEvent e, ActivatedAssembledTankNode activatedTank)
        {
            activatedTank.tankVisualRoot.gameObject.AddComponent<TargetBehaviour>();
            activatedTank.rigidbody.Rigidbody.gameObject.AddComponent<TargetBehaviour>();
        }

        [OnEventComplete]
        public void AssembleTank(NodeAddedEvent e, TankNode tank, [Context, JoinByTank] WeaponNode weapon)
        {
            GameObject assemblyRoot = new GameObject("Tank " + tank.hullInstance.HullInstance.name + "/" + weapon.weaponInstance.WeaponInstance.name);
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            Transform parent = tank.hullInstance.HullInstance.transform;
            transform.parent = parent;
            UnityUtil.InheritAndEmplace(tank.tankCommonInstance.TankCommonInstance.transform, parent);
            tank.tankPartPaintInstance.PaintInstance.transform.parent = parent;
            parent.parent = assemblyRoot.transform;
            transform.SetParent(tank.mountPoint.MountPoint, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            assemblyRoot.SetActive(false);
            tank.Entity.AddComponent(new AssembledTankComponent(assemblyRoot));
        }

        [OnEventComplete]
        public void ConfigureTargetBehaviour(NodeAddedEvent e, TankIncarnationNode tankIncarnation, [JoinByTank, Context] ActivatedAssembledTankNode activatedTank)
        {
            activatedTank.tankVisualRoot.gameObject.GetComponent<TargetBehaviour>().Init(activatedTank.Entity, tankIncarnation.Entity);
            activatedTank.rigidbody.Rigidbody.gameObject.GetComponent<TargetBehaviour>().Init(activatedTank.Entity, tankIncarnation.Entity);
        }

        [OnEventComplete]
        public void DestroyTank(NodeRemoveEvent e, AssembledTankNode tank)
        {
            UnityUtil.Destroy(tank.hullInstance.HullInstance);
            UnityUtil.Destroy(tank.assembledTank.AssemblyRoot);
        }

        [OnEventComplete]
        public void DestroyTankHullIfTankNotAssembled(NodeRemoveEvent e, SingleNode<HullInstanceComponent> tank)
        {
            if (!tank.Entity.HasComponent<AssembledTankComponent>())
            {
                UnityUtil.Destroy(tank.component.HullInstance);
            }
        }

        [OnEventComplete]
        public void DestroyTankWeaponIfTankNotAssembled(NodeRemoveEvent e, SingleNode<HullInstanceComponent> tank, [JoinByUser] WeaponTankPartNode weapon)
        {
            if (!tank.Entity.HasComponent<AssembledTankComponent>())
            {
                GameObject weaponInstance = weapon.weaponInstance.WeaponInstance;
                if (weaponInstance)
                {
                    Object.Destroy(weaponInstance);
                }
            }
        }

        public class ActivatedAssembledTankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public AssembledTankComponent assembledTank;
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
            public TankVisualRootComponent tankVisualRoot;
        }

        public class AssembledActivationTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
            public AssembledTankComponent assembledTank;
            public TankCommonInstanceComponent tankCommonInstance;
            public HullInstanceComponent hullInstance;
            public MountPointComponent mountPoint;
            public TankPartPaintInstanceComponent tankPartPaintInstance;
            public AssembledTankInactiveStateComponent assembledTankInactiveState;
        }

        public class AssembledTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public AssembledTankComponent assembledTank;
            public HullInstanceComponent hullInstance;
        }

        public class TankIncarnationNode : Node
        {
            public TankClientIncarnationComponent tankClientIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankCommonInstanceComponent tankCommonInstance;
            public HullInstanceComponent hullInstance;
            public MountPointComponent mountPoint;
            public TankPartPaintInstanceComponent tankPartPaintInstance;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public TankPartPaintInstanceComponent tankPartPaintInstance;
        }

        public class WeaponTankPartNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public TankPartComponent tankPart;
        }
    }
}

