namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class RicochetSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InstantiateEffectOnStaticHit(RicochetBulletBounceEvent e, BulletNode bullet, [JoinByTank] RicochetSoundEffectNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Vector3 worldSpaceBouncePosition = e.WorldSpaceBouncePosition;
            weapon.ricochetBounceSoundEffect.PlayEffect(worldSpaceBouncePosition);
        }

        [OnEventFire]
        public void InstantiateEffectOnTargetHit(BulletHitEvent e, BulletNode bullet, [JoinByTank] RicochetSoundEffectNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Vector3 position = e.Position;
            weapon.ricochetTargetHitSoundEffect.PlayEffect(position);
        }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public RicochetBulletComponent ricochetBullet;
        }

        public class RicochetSoundEffectNode : Node
        {
            public RicochetBounceSoundEffectComponent ricochetBounceSoundEffect;
            public RicochetTargetHitSoundEffectComponent ricochetTargetHitSoundEffect;
            public TankGroupComponent tankGroup;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

