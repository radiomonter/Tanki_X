namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class RemoteShaftAimingLaserSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanLaserOnTankDeath(NodeRemoveEvent evt, AimingReadyLaserForNRNode nr, [JoinSelf] AimingReadyLaserNode weapon)
        {
            foreach (ShaftAimingLaserBehaviour behaviour in weapon.shaftAimingLaser.EffectInstances)
            {
                behaviour.Kill();
            }
            weapon.Entity.RemoveComponent<ShaftAimingLaserReadyComponent>();
        }

        [OnEventFire]
        public void HideLaser(NodeRemoveEvent evt, AimingLaserTargetPointNode weapon)
        {
            weapon.shaftAimingLaser.EffectInstance.Hide();
        }

        [OnEventFire]
        public void InstantiateLaserForRemoteTank(NodeAddedEvent evt, ShaftAimingLaserNode weapon, [Context, JoinByTank] RemoteTankNode remoteTank)
        {
            if (remoteTank.assembledTank.AssemblyRoot)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(weapon.shaftAimingLaser.Asset);
                ShaftAimingLaserBehaviour component = gameObject.GetComponent<ShaftAimingLaserBehaviour>();
                weapon.shaftAimingLaser.EffectInstance = component;
                CustomRenderQueue.SetQueue(gameObject, 0xc4e);
                Transform transform = weapon.shaftAimingLaserSource.gameObject.transform;
                gameObject.transform.position = transform.position;
                gameObject.transform.rotation = transform.rotation;
                component.Init();
                component.SetColor(weapon.shaftAimingColorEffect.ChoosenColor);
                weapon.Entity.AddComponent<ShaftAimingLaserReadyComponent>();
            }
        }

        private ShaftAimingLaserBehaviour InterpolateLaser(AimingLaserTargetPointNode weapon, Vector3 startPosition, Vector3 laserDir)
        {
            bool flag;
            bool flag2;
            Vector3 vector3;
            Vector3 leftDirectionWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetLeftDirectionWorld();
            laserDir = Vector3.ProjectOnPlane(laserDir, leftDirectionWorld).normalized;
            ShaftAimingLaserBehaviour effectInstance = weapon.shaftAimingLaser.EffectInstance;
            float maxLength = weapon.shaftAimingLaser.MaxLength;
            float minLength = weapon.shaftAimingLaser.MinLength;
            DirectionData data = weapon.targetCollector.Collect(startPosition, laserDir, maxLength, LayerMasks.VISUAL_TARGETING);
            if (data.HasAnyHit())
            {
                flag = true;
                flag2 = data.FirstAnyHitDistance() >= minLength;
                vector3 = data.FirstAnyHitPosition();
            }
            else
            {
                flag = false;
                flag2 = true;
                vector3 = startPosition + (laserDir * maxLength);
            }
            bool flag3 = !weapon.shaftAimingTargetPoint.IsInsideTankPart;
            effectInstance.UpdateTargetPosition(weapon.shaftAimingLaserSource.transform.position, vector3, flag2 & flag3, flag & flag3);
            weapon.shaftAimingLaser.CurrentLaserDirection = laserDir;
            return effectInstance;
        }

        [OnEventFire]
        public void ShowAndInitLaser(NodeAddedEvent evt, AimingLaserTargetPointNode weapon, [Context, JoinByTank] ActiveTankNode tank, [JoinAll, Context] CameraNode camera)
        {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            this.InterpolateLaser(weapon, barrelOriginWorld, Vector3.Normalize(weapon.shaftAimingTargetPoint.Point - barrelOriginWorld)).Show();
        }

        [OnEventFire]
        public void UpdateLaserTargetPosition(UpdateEvent evt, AimingLaserTargetPointNode weapon, [JoinByTank] RemoteTankNode tank, [JoinAll] CameraNode camera)
        {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            this.InterpolateLaser(weapon, barrelOriginWorld, Vector3.Lerp(weapon.shaftAimingLaser.CurrentLaserDirection, Vector3.Normalize(weapon.shaftAimingTargetPoint.Point - barrelOriginWorld), weapon.shaftAimingLaser.InterpolationCoeff).normalized);
        }

        public class ActiveTankNode : RemoteShaftAimingLaserSystem.RemoteTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class AimingLaserTargetPointNode : RemoteShaftAimingLaserSystem.AimingReadyLaserNode
        {
            public MuzzlePointComponent muzzlePoint;
            public WeaponUnblockedComponent weaponUnblocked;
            public ShaftAimingTargetPointComponent shaftAimingTargetPoint;
            public TargetCollectorComponent targetCollector;
        }

        public class AimingReadyLaserForNRNode : Node
        {
            public ShaftAimingLaserComponent shaftAimingLaser;
            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;
            public TankGroupComponent tankGroup;
        }

        public class AimingReadyLaserNode : RemoteShaftAimingLaserSystem.ShaftAimingLaserNode
        {
            public ShaftAimingLaserReadyComponent shaftAimingLaserReady;
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class RemoteTankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankCollidersComponent tankColliders;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public AssembledTankComponent assembledTank;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingLaserNode : Node
        {
            public ShaftAimingLaserComponent shaftAimingLaser;
            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;
            public ShaftAimingColorEffectPreparedComponent shaftAimingColorEffectPrepared;
            public TankGroupComponent tankGroup;
        }
    }
}

