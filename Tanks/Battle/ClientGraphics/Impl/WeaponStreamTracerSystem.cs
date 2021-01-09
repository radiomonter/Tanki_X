namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class WeaponStreamTracerSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, WeaponStreamTracerInitNode weapon)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(weapon.weaponStreamTracerEffect.Tracer);
            UnityUtil.InheritAndEmplace(obj2.transform, weapon.muzzlePoint.Current);
            obj2.SetActive(false);
            Transform transform = obj2.transform;
            transform.localPosition += Vector3.forward * weapon.weaponStreamTracerEffect.StartTracerOffset;
            weapon.weaponStreamTracerEffect.Tracer = obj2;
            weapon.Entity.AddComponent<WeaponStreamTracerEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartTracer(NodeAddedEvent evt, WeaponStreamTracerShootingEffectNode weapon)
        {
            weapon.weaponStreamTracerEffect.Tracer.SetActive(true);
        }

        [OnEventFire]
        public void StopTracer(NodeRemoveEvent evt, WeaponStreamTracerShootingEffectNode node)
        {
            node.weaponStreamTracerEffect.Tracer.SetActive(false);
        }

        [OnEventComplete]
        public void UpdateTracer(UpdateEvent evt, WeaponStreamTracerShootingEffectNode weapon)
        {
            WeaponStreamTracerBehaviour component = weapon.weaponStreamTracerEffect.Tracer.GetComponent<WeaponStreamTracerBehaviour>();
            if (!weapon.Entity.HasComponent<StreamHitComponent>())
            {
                component.TargetPosition = new Vector3(0f, 0f, weapon.weaponStreamTracerEffect.TracerMaxLength);
            }
            else
            {
                base.ScheduleEvent<UpdateWeaponStreamTracerByStreamHitEvent>(weapon);
            }
        }

        [OnEventFire]
        public void UpdateTracer(UpdateWeaponStreamTracerByStreamHitEvent evt, WeaponStreamTracerStreamHitNode weapon)
        {
            StreamHitComponent streamHit = weapon.streamHit;
            GameObject tracer = weapon.weaponStreamTracerEffect.Tracer;
            WeaponStreamTracerBehaviour component = tracer.GetComponent<WeaponStreamTracerBehaviour>();
            if (streamHit.StaticHit != null)
            {
                component.TargetPosition = MathUtil.WorldPositionToLocalPosition(streamHit.StaticHit.Position, tracer);
            }
            else if ((streamHit.TankHit != null) && weapon.Entity.HasComponent<StreamHitTargetLoadedComponent>())
            {
                UpdateWeaponStreamTracerByTargetTankEvent eventInstance = new UpdateWeaponStreamTracerByTargetTankEvent {
                    WeaponStreamTracerBehaviour = component,
                    Hit = streamHit.TankHit,
                    WeaponStreamTracerInstance = tracer
                };
                base.ScheduleEvent(eventInstance, streamHit.TankHit.Entity);
            }
        }

        [OnEventFire]
        public void UpdateTracer(UpdateWeaponStreamTracerByTargetTankEvent evt, HullNode tank)
        {
            GameObject hullInstance = tank.hullInstance.HullInstance;
            Vector3 position = MathUtil.LocalPositionToWorldPosition(evt.Hit.LocalHitPoint, hullInstance);
            evt.WeaponStreamTracerBehaviour.TargetPosition = MathUtil.WorldPositionToLocalPosition(position, evt.WeaponStreamTracerInstance);
        }

        public class HullNode : Node
        {
            public HullInstanceComponent hullInstance;
            public NewHolyshieldEffectComponent newHolyshieldEffect;
        }

        public class WeaponStreamTracerInitNode : Node
        {
            public WeaponStreamTracerEffectComponent weaponStreamTracerEffect;
            public MuzzlePointComponent muzzlePoint;
            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class WeaponStreamTracerShootingEffectNode : Node
        {
            public WeaponStreamTracerEffectReadyComponent weaponStreamTracerEffectReady;
            public WeaponStreamTracerEffectComponent weaponStreamTracerEffect;
            public WeaponUnblockedComponent weaponUnblocked;
            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class WeaponStreamTracerStreamHitNode : Node
        {
            public WeaponStreamTracerEffectReadyComponent weaponStreamTracerEffectReady;
            public WeaponStreamTracerEffectComponent weaponStreamTracerEffect;
            public StreamHitComponent streamHit;
        }
    }
}

