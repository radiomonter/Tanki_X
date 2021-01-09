namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponStreamHitSoundsSystem : ECSSystem
    {
        private void InitStreamHitWeaponEffect<T>(Entity weapon, BaseStreamHitWeaponSoundEffect effect, Transform root) where T: Component, new()
        {
            effect.SoundController = Object.Instantiate<GameObject>(effect.EffectPrefab, root.position, root.rotation, root.transform).GetComponent<SoundController>();
            weapon.AddComponent<T>();
        }

        [OnEventFire]
        public void InitWeaponShootingSound(NodeAddedEvent e, WeaponShootingSoundInitNode weapon)
        {
            this.InitStreamHitWeaponEffect<WeaponShootingSoundEffectReadyComponent>(weapon.Entity, weapon.weaponShootingSoundEffect, weapon.weaponSoundRoot.transform);
        }

        [OnEventFire]
        public void InitWeaponStreamHitSound(NodeAddedEvent e, WeaponStreamHitSoundNode weapon)
        {
            this.InitStreamHitWeaponEffect<WeaponStreamHitSoundsEffectReadyComponent>(weapon.Entity, weapon.weaponStreamHitSoundsEffect, weapon.weaponSoundRoot.transform);
        }

        [OnEventFire]
        public void Play(NodeAddedEvent e, WeaponShootingStateSoundReadyNode weapon)
        {
            weapon.weaponShootingSoundEffect.SoundController.FadeIn();
        }

        [OnEventFire]
        public void StartHitSounds(NodeAddedEvent evt, WeaponStreamHitSoundActiveNode weapon)
        {
            this.UpdateHitSoundsByForce(weapon.weaponStreamHitSoundsEffect, weapon.streamHit);
            this.UpdateHitSoundPosition(weapon.weaponStreamHitSoundsEffect, weapon.streamHit);
        }

        [OnEventFire]
        public void Stop(NodeRemoveEvent e, WeaponShootingStateSoundReadyNode weapon)
        {
            weapon.weaponShootingSoundEffect.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StopHitSounds(NodeRemoveEvent evt, WeaponStreamHitSoundActiveNode weapon)
        {
            if (weapon.weaponStreamHitSoundsEffect.SoundController)
            {
                weapon.weaponStreamHitSoundsEffect.SoundController.StopImmediately();
                weapon.weaponStreamHitSoundsEffect.SoundController.gameObject.transform.localPosition = Vector3.zero;
            }
        }

        [OnEventComplete]
        public void UpdateHitSound(UpdateEvent evt, WeaponStreamHitSoundActiveNode weapon)
        {
            this.UpdateHitSoundsIfNeeded(weapon.weaponStreamHitSoundsEffect, weapon.streamHit);
            this.UpdateHitSoundPosition(weapon.weaponStreamHitSoundsEffect, weapon.streamHit);
        }

        private void UpdateHitSoundPosition(WeaponStreamHitSoundsEffectComponent effect, StreamHitComponent hit)
        {
            bool flag2 = !ReferenceEquals(hit.TankHit, null);
            if (!ReferenceEquals(hit.StaticHit, null))
            {
                effect.SoundController.gameObject.transform.position = hit.StaticHit.Position;
            }
            if (flag2)
            {
                effect.SoundController.gameObject.transform.position = hit.TankHit.TargetPosition;
            }
        }

        private void UpdateHitSoundsByForce(WeaponStreamHitSoundsEffectComponent effect, StreamHitComponent hit)
        {
            bool flag = !ReferenceEquals(hit.StaticHit, null);
            effect.IsStaticHit = flag;
            effect.SoundController.StopImmediately();
            effect.SoundController.Source.clip = !flag ? effect.TargetHitClip : effect.StaticHitClip;
            effect.SoundController.SetSoundActive();
        }

        private void UpdateHitSoundsIfNeeded(WeaponStreamHitSoundsEffectComponent effect, StreamHitComponent hit)
        {
            bool flag = !ReferenceEquals(hit.StaticHit, null);
            if (effect.IsStaticHit != flag)
            {
                this.UpdateHitSoundsByForce(effect, hit);
            }
        }

        public class WeaponShootingSoundInitNode : Node
        {
            public WeaponShootingSoundEffectComponent weaponShootingSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class WeaponShootingSoundReadyNode : WeaponStreamHitSoundsSystem.WeaponShootingSoundInitNode
        {
            public WeaponShootingSoundEffectReadyComponent weaponShootingSoundEffectReady;
        }

        public class WeaponShootingStateSoundReadyNode : WeaponStreamHitSoundsSystem.WeaponShootingSoundReadyNode
        {
            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class WeaponStreamHitSoundActiveNode : WeaponStreamHitSoundsSystem.WeaponStreamHitSoundReadyNode
        {
            public StreamHitComponent streamHit;
        }

        public class WeaponStreamHitSoundNode : Node
        {
            public WeaponStreamHitSoundsEffectComponent weaponStreamHitSoundsEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class WeaponStreamHitSoundReadyNode : WeaponStreamHitSoundsSystem.WeaponStreamHitSoundNode
        {
            public WeaponStreamHitSoundsEffectReadyComponent weaponStreamHitSoundsEffectReady;
        }
    }
}

