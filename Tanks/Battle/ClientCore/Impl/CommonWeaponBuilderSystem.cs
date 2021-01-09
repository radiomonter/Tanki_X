namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class CommonWeaponBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void AssembleWeaponWithShell(NodeAddedEvent evt, WeaponInstanceNode weapon, [Context, JoinByTank] ShellInstanceNode shell)
        {
            Transform transform = shell.shellInstance.ShellInstance.transform;
            GameObject weaponInstance = weapon.weaponInstance.WeaponInstance;
            transform.parent = weaponInstance.GetComponentInChildren<WeaponVisualRootComponent>().transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            this.BuildWeaponEntity(weaponInstance, weapon.Entity);
            weapon.Entity.AddComponent<WeaponPreparedByEntityBehaviourComponent>();
        }

        [OnEventFire]
        public void BuildShell(BuildWeaponShellEvent e, ShellNode shell)
        {
            GameObject shellInstance = this.CreateInstance(shell.resourceData);
            shellInstance.SetActive(true);
            if (shell.Entity.HasComponent<ShellInstanceComponent>())
            {
                shell.Entity.GetComponent<ShellInstanceComponent>().ShellInstance = shellInstance;
            }
            else
            {
                shell.Entity.AddComponent(new ShellInstanceComponent(shellInstance));
            }
        }

        [OnEventFire]
        public void BuildShell(NodeAddedEvent evt, [Combine] ShellNode shell, SingleNode<MapInstanceComponent> map)
        {
            base.NewEvent<BuildWeaponShellEvent>().Attach(shell).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void BuildWeapon(WeaponInstanceIsReadyEvent evt, WeaponNode weapon)
        {
            GameObject weaponInstance = evt.WeaponInstance;
            weapon.Entity.AddComponent(new WeaponInstanceComponent(weaponInstance));
            weaponInstance.AddComponent<NanFixer>().Init(null, weaponInstance.transform, weapon.Entity.GetComponent<UserGroupComponent>().Key);
        }

        [OnEventFire]
        public void BuildWeapon(NodeAddedEvent evt, [Combine] WeaponSkinNode skin, [Context, JoinByTank] WeaponNode weapon, SingleNode<MapInstanceComponent> map)
        {
            WeaponInstanceIsReadyEvent eventInstance = new WeaponInstanceIsReadyEvent {
                WeaponInstance = this.CreateInstance(skin.resourceData)
            };
            base.NewEvent(eventInstance).Attach(weapon).ScheduleDelayed(0.3f);
        }

        private void BuildWeaponEntity(GameObject weaponInstance, Entity weaponEntity)
        {
            weaponInstance.GetComponent<EntityBehaviour>().BuildEntity(weaponEntity);
            PhysicsUtil.SetGameObjectLayer(weaponInstance, Layers.INVISIBLE_PHYSICS);
        }

        private GameObject CreateInstance(ResourceDataComponent resourceData)
        {
            GameObject obj2 = (GameObject) Object.Instantiate(resourceData.Data);
            obj2.SetActive(false);
            return obj2;
        }

        public class ShellInstanceNode : Node
        {
            public ShellInstanceComponent shellInstance;
            public TankGroupComponent tankGroup;
        }

        public class ShellNode : Node
        {
            public ShellBattleItemComponent shellBattleItem;
            public ResourceDataComponent resourceData;
        }

        public class WeaponInstanceIsReadyEvent : Event
        {
            public GameObject WeaponInstance;
        }

        public class WeaponInstanceNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }

        public class WeaponSkinNode : Node
        {
            public WeaponSkinBattleItemComponent weaponSkinBattleItem;
            public ResourceDataComponent resourceData;
            public TankGroupComponent tankGroup;
        }
    }
}

