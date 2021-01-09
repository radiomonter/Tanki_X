namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class WeaponStreamHitGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, WeaponStreamHitGraphicsEffectInitNode weapon)
        {
            weapon.weaponStreamHitGraphicsEffect.Init(weapon.muzzlePoint.Current);
            weapon.Entity.AddComponent<WeaponStreamHitGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void StopHitEffect(NodeRemoveEvent evt, WeaponStreamHitGraphicsNode weapon)
        {
            WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect = weapon.weaponStreamHitGraphicsEffect;
            if (weaponStreamHitGraphicsEffect.HitStatic)
            {
                weaponStreamHitGraphicsEffect.HitStatic.Stop(true);
            }
            if (weaponStreamHitGraphicsEffect.HitTarget != null)
            {
                weaponStreamHitGraphicsEffect.HitTarget.Stop(true);
            }
            if (weaponStreamHitGraphicsEffect.HitStaticLight != null)
            {
                weaponStreamHitGraphicsEffect.HitStaticLight.enabled = false;
            }
            if (weaponStreamHitGraphicsEffect.HitTargetLight != null)
            {
                weaponStreamHitGraphicsEffect.HitTargetLight.enabled = false;
            }
        }

        [OnEventComplete]
        public void UpdateHitEffect(UpdateEvent evt, WeaponStreamHitGraphicsNode weapon)
        {
            WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect = weapon.weaponStreamHitGraphicsEffect;
            StreamHitComponent streamHit = weapon.streamHit;
            if (streamHit.StaticHit != null)
            {
                weaponStreamHitGraphicsEffect.HitStatic.transform.position = streamHit.StaticHit.Position + (streamHit.StaticHit.Normal * weaponStreamHitGraphicsEffect.HitOffset);
                weaponStreamHitGraphicsEffect.HitStatic.transform.rotation = Quaternion.LookRotation(streamHit.StaticHit.Normal);
                weaponStreamHitGraphicsEffect.HitStatic.Play(true);
                weaponStreamHitGraphicsEffect.HitStaticLight.enabled = true;
            }
            else if ((streamHit.TankHit != null) && weapon.Entity.HasComponent<StreamHitTargetLoadedComponent>())
            {
                UpdateWeaponStreamHitGraphicsByTargetTankEvent eventInstance = new UpdateWeaponStreamHitGraphicsByTargetTankEvent {
                    HitTargetParticleSystem = weaponStreamHitGraphicsEffect.HitTarget,
                    HitTargetLight = weaponStreamHitGraphicsEffect.HitTargetLight,
                    TankHit = streamHit.TankHit,
                    HitOffset = weaponStreamHitGraphicsEffect.HitOffset
                };
                base.ScheduleEvent(eventInstance, streamHit.TankHit.Entity);
            }
        }

        [OnEventFire]
        public void UpdateHitEffect(UpdateWeaponStreamHitGraphicsByTargetTankEvent evt, HullNode tank)
        {
            Vector3 vector = tank.hullInstance.HullInstance.transform.TransformPoint(evt.TankHit.LocalHitPoint);
            Quaternion quaternion = Quaternion.LookRotation(evt.TankHit.HitDirection);
            evt.HitTargetParticleSystem.transform.position = vector - (evt.TankHit.HitDirection * evt.HitOffset);
            evt.HitTargetParticleSystem.transform.rotation = quaternion;
            evt.HitTargetParticleSystem.Play(true);
            evt.HitTargetLight.enabled = true;
        }

        public class HullNode : Node
        {
            public HullInstanceComponent hullInstance;
            public NewHolyshieldEffectComponent newHolyshieldEffect;
        }

        public class WeaponStreamHitGraphicsEffectInitNode : Node
        {
            public WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect;
            public MuzzlePointComponent muzzlePoint;
        }

        public class WeaponStreamHitGraphicsNode : Node
        {
            public WeaponStreamHitGraphicsEffectReadyComponent weaponStreamHitGraphicsEffectReady;
            public WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect;
            public StreamHitComponent streamHit;
        }
    }
}

