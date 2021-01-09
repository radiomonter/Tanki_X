namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HammerHitSoundEffectSystem : BaseHitExplosionSoundSystem
    {
        [OnEventFire]
        public void CreateHitSoundEffect(HitEvent evt, HammerHitSoundEffectNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            List<HitTarget> targets = evt.Targets;
            HammerHitSoundEffectComponent hammerHitSoundEffect = weapon.hammerHitSoundEffect;
            if (targets != null)
            {
                int count = targets.Count;
                if (count > 0)
                {
                    List<HitTarget> differentTargetsByHit = hammerHitSoundEffect.DifferentTargetsByHit;
                    differentTargetsByHit.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        HitTarget item = targets[i];
                        if (!differentTargetsByHit.Contains(item))
                        {
                            differentTargetsByHit.Add(item);
                        }
                    }
                    Vector3 zero = Vector3.zero;
                    int num3 = differentTargetsByHit.Count;
                    for (int j = 0; j < num3; j++)
                    {
                        zero += differentTargetsByHit[j].TargetPosition;
                    }
                    zero /= (float) num3;
                    base.CreateHitExplosionSoundEffect(zero, hammerHitSoundEffect.TargetHitSoundAsset, hammerHitSoundEffect.TargetHitSoundDuration);
                    return;
                }
            }
            if (evt.StaticHit != null)
            {
                GameObject staticHitSoundAsset = hammerHitSoundEffect.StaticHitSoundAsset;
                base.CreateHitExplosionSoundEffect(evt.StaticHit.Position, staticHitSoundAsset, hammerHitSoundEffect.StaticHitSoundDuration);
            }
        }

        public class HammerHitSoundEffectNode : Node
        {
            public HammerHitSoundEffectComponent hammerHitSoundEffect;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

