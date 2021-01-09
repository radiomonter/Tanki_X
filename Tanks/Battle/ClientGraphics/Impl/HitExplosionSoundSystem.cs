namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HitExplosionSoundSystem : BaseHitExplosionSoundSystem
    {
        [OnEventFire]
        public void CreateHitSoundEffect(HitEvent evt, HitExplosionSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            HitExplosionSoundComponent hitExplosionSound = weapon.hitExplosionSound;
            GameObject soundPrefab = hitExplosionSound.SoundPrefab;
            float duration = hitExplosionSound.Duration;
            if (evt.Targets != null)
            {
                foreach (HitTarget target in evt.Targets)
                {
                    base.CreateHitExplosionSoundEffect(target.TargetPosition, soundPrefab, duration);
                }
            }
            if (evt.StaticHit != null)
            {
                base.CreateHitExplosionSoundEffect(evt.StaticHit.Position, soundPrefab, duration);
            }
        }

        [OnEventFire]
        public void Explosion(BulletHitEvent e, Node node, [JoinByTank] HitExplosionSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            HitExplosionSoundComponent hitExplosionSound = weapon.hitExplosionSound;
            GameObject soundPrefab = hitExplosionSound.SoundPrefab;
            base.CreateHitExplosionSoundEffect(e.Position, soundPrefab, hitExplosionSound.Duration);
        }

        public class HitExplosionSoundNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public HitExplosionSoundComponent hitExplosionSound;
            public TankGroupComponent tankGroup;
        }
    }
}

