namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class DiscreteWeaponShotEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void Build(NodeAddedEvent evt, [Combine] DiscreteWeaponSoundEffectNode node, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            DiscreteWeaponShotEffectComponent discreteWeaponShotEffect = node.discreteWeaponShotEffect;
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            discreteWeaponShotEffect.AudioSources = new AudioSource[muzzlePoint.Points.Length];
            for (int i = 0; i < muzzlePoint.Points.Length; i++)
            {
                GameObject obj2 = Object.Instantiate<GameObject>(discreteWeaponShotEffect.Asset);
                discreteWeaponShotEffect.AudioSources[i] = obj2.GetComponent<AudioSource>();
                obj2.transform.parent = node.weaponSoundRoot.gameObject.transform;
                obj2.transform.SetPositionSafe(muzzlePoint.Points[i].position);
            }
            if (!node.Entity.HasComponent<DiscreteWeaponShotEffectReadyComponent>())
            {
                node.Entity.AddComponent<DiscreteWeaponShotEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void PlayShotEffect(BaseShotEvent evt, DiscreteWeaponSoundEffectReadyNode node)
        {
            DiscreteWeaponShotEffectComponent discreteWeaponShotEffect = node.discreteWeaponShotEffect;
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            discreteWeaponShotEffect.AudioSources[muzzlePoint.CurrentIndex].Stop();
            discreteWeaponShotEffect.AudioSources[muzzlePoint.CurrentIndex].Play();
        }

        public class DiscreteWeaponSoundEffectNode : Node
        {
            public DiscreteWeaponComponent discreteWeapon;
            public DiscreteWeaponShotEffectComponent discreteWeaponShotEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public MuzzlePointComponent muzzlePoint;
            public AnimationPreparedComponent animationPrepared;
        }

        public class DiscreteWeaponSoundEffectReadyNode : DiscreteWeaponShotEffectSystem.DiscreteWeaponSoundEffectNode
        {
            public DiscreteWeaponShotEffectReadyComponent discreteWeaponShotEffectReady;
        }
    }
}

