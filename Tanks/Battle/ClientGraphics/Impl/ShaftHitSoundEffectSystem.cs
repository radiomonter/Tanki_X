namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShaftHitSoundEffectSystem : ECSSystem
    {
        private void CreateShaftHitSoundEffect(HitEvent evt, ShaftHitSoundEffectComponent effectComponent)
        {
            if (evt.Targets != null)
            {
                foreach (HitTarget target in evt.Targets)
                {
                    this.CreateShaftHitSoundEffect(target.TargetPosition, effectComponent);
                }
            }
            if (evt.StaticHit != null)
            {
                this.CreateShaftHitSoundEffect(evt.StaticHit.Position, effectComponent);
            }
        }

        private void CreateShaftHitSoundEffect(Vector3 position, ShaftHitSoundEffectComponent effectComponent)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = effectComponent.Asset,
                AutoRecycleTime = effectComponent.Duration
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            eventInstance.Instance.position = position;
            eventInstance.Instance.rotation = Quaternion.identity;
        }

        [OnEventFire]
        public void CreateShaftQuickHitSelfSoundEffect(SelfShaftAimingHitEvent evt, SingleNode<ShaftAimingHitSoundEffectComponent> weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.CreateShaftHitSoundEffect(evt, weapon.component);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteHitEvent evt, ShaftQuickHitSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.CreateShaftHitSoundEffect(evt, weapon.shaftQuickHitSoundEffect);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteShaftAimingHitEvent evt, SingleNode<ShaftAimingHitSoundEffectComponent> weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.CreateShaftHitSoundEffect(evt, weapon.component);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(SelfHitEvent evt, ShaftQuickHitSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.CreateShaftHitSoundEffect(evt, weapon.shaftQuickHitSoundEffect);
        }

        [Not(typeof(ShaftAimingWorkFinishStateComponent))]
        public class ShaftQuickHitSoundNode : Node
        {
            public ShaftQuickHitSoundEffectComponent shaftQuickHitSoundEffect;
        }
    }
}

