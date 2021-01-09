namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShotValidateSystem : ECSSystem
    {
        private const float ADDITIONAL_GUN_LENGTH = 0.1f;
        private HashSet<Type> weaponStates;

        public ShotValidateSystem()
        {
            HashSet<Type> set = new HashSet<Type> {
                typeof(WeaponUnblockedComponent),
                typeof(WeaponUndergroundComponent),
                typeof(WeaponBlockedComponent)
            };
            this.weaponStates = set;
        }

        private void AddOrChangeWeaponBlockedComponent(Entity weapon, RaycastHit hitInfo)
        {
            WeaponBlockedComponent component = !weapon.HasComponent<WeaponBlockedComponent>() ? ((WeaponBlockedComponent) weapon.CreateNewComponentInstance(typeof(WeaponBlockedComponent))) : weapon.GetComponent<WeaponBlockedComponent>();
            component.BlockPoint = PhysicsUtil.GetPulledHitPoint(hitInfo);
            component.BlockGameObject = hitInfo.collider.gameObject;
            component.BlockNormal = hitInfo.normal;
            StateUtils.SwitchEntityState(weapon, component, this.weaponStates);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, ActiveTankNode activeTank, [JoinByTank, Context] WeaponNode weaponNode)
        {
            StateUtils.SwitchEntityState<WeaponUnblockedComponent>(weaponNode.Entity, this.weaponStates);
        }

        private bool IsWeaponBlocked(MuzzleLogicAccessor muzzlePoint, int mask, out RaycastHit hitInfo, GameObject[] raycastExclusionGameObjects)
        {
            Vector3 worldPosition = muzzlePoint.GetWorldPosition();
            Vector3 barrelOriginWorld = muzzlePoint.GetBarrelOriginWorld();
            float distance = (worldPosition - barrelOriginWorld).magnitude + 0.1f;
            return PhysicsUtil.RaycastWithExclusion(barrelOriginWorld, worldPosition - barrelOriginWorld, out hitInfo, distance, mask, raycastExclusionGameObjects);
        }

        private bool IsWeaponUnderground(MuzzleLogicAccessor muzzlePoint, int mask, TankNode tank, GameObject[] raycastExclusionGameObjects)
        {
            RaycastHit hit;
            Vector3 center = tank.tankColliders.BoundsCollider.bounds.center;
            Vector3 dir = muzzlePoint.GetBarrelOriginWorld() - center;
            return PhysicsUtil.RaycastWithExclusion(center, dir, out hit, dir.magnitude, mask, raycastExclusionGameObjects);
        }

        [OnEventFire]
        public void ValidateShot(TimeUpdateEvent evt, WeaponMuzzleNode weaponNode, [JoinByTank] TankNode tank)
        {
            this.ValidateShot(weaponNode.Entity, new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance), tank, weaponNode.shotValidate);
        }

        private void ValidateShot(Entity weapon, MuzzleLogicAccessor muzzlePoint, TankNode tank, ShotValidateComponent shotValidate)
        {
            if (this.IsWeaponUnderground(muzzlePoint, shotValidate.UnderGroundValidateMask, tank, shotValidate.RaycastExclusionGameObjects))
            {
                StateUtils.SwitchEntityState<WeaponUndergroundComponent>(weapon, this.weaponStates);
            }
            else
            {
                RaycastHit hit;
                if (this.IsWeaponBlocked(muzzlePoint, shotValidate.BlockValidateMask, out hit, shotValidate.RaycastExclusionGameObjects))
                {
                    this.AddOrChangeWeaponBlockedComponent(weapon, hit);
                }
                else
                {
                    StateUtils.SwitchEntityState<WeaponUnblockedComponent>(weapon, this.weaponStates);
                }
            }
        }

        [OnEventFire]
        public void ValidateShotBeforeShot(BeforeShotEvent evt, WeaponMuzzleNode weaponNode, [JoinByTank] TankNode tank)
        {
            this.ValidateShot(weaponNode.Entity, new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance), tank, weaponNode.shotValidate);
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankCollidersComponent tankColliders;
            public MountPointComponent mountPoint;
            public TankGroupComponent tankGroup;
        }

        public class WeaponMuzzleNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
            public ShotValidateComponent shotValidate;
            public WeaponInstanceComponent weaponInstance;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
        }
    }
}

