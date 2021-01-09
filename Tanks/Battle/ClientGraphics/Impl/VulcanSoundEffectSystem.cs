namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class VulcanSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitEffects(NodeAddedEvent evt, [Combine] VulcanAllSoundEffectsNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRoot = weapon.weaponSoundRoot.transform;
            VulcanSoundManagerComponent soundManagerComponent = new VulcanSoundManagerComponent();
            this.InitSoundEffect(weapon.vulcanAfterShootingSoundEffect, soundManagerComponent, soundRoot);
            this.InitSoundEffect(weapon.vulcanChainStartSoundEffect, soundManagerComponent, soundRoot);
            this.InitSoundEffect(weapon.vulcanSlowDownAfterSpeedUpSoundEffect, soundManagerComponent, soundRoot);
            this.InitSoundEffect(weapon.vulcanTurbineSoundEffect, soundManagerComponent, soundRoot);
            float length = weapon.vulcanTurbineSoundEffect.SoundSource.clip.length;
            weapon.vulcanTurbineSoundEffect.StartTimePerSec = length - weapon.vulcanWeapon.SpeedUpTime;
            weapon.vulcanTurbineSoundEffect.SoundSource.time = weapon.vulcanTurbineSoundEffect.StartTimePerSec;
            weapon.Entity.AddComponent(soundManagerComponent);
        }

        private void InitSoundEffect(AbstractVulcanSoundEffectComponent soundEffectComponent, VulcanSoundManagerComponent soundManagerComponent, Transform soundRoot)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(soundEffectComponent.EffectPrefab);
            obj2.transform.parent = soundRoot;
            obj2.transform.localPosition = Vector3.zero;
            AudioSource component = obj2.GetComponent<AudioSource>();
            soundEffectComponent.SoundSource = component;
            component.time = soundEffectComponent.StartTimePerSec;
            soundManagerComponent.SoundsWithDelay.Add(component, soundEffectComponent.DelayPerSec);
        }

        private void PlayNextSound(AudioSource sound, VulcanSoundManagerComponent manager)
        {
            this.StopCurrentSound(manager);
            if (sound != null)
            {
                manager.CurrentSound = sound;
                this.PlaySound(sound, manager);
            }
        }

        [OnEventFire]
        public void PlayShootingProcessEffect(NodeAddedEvent evt, VulcanShootingNode weapon)
        {
            this.PlayNextSound(null, weapon.vulcanSoundManager);
        }

        [OnEventFire]
        public void PlaySlowDownSound(NodeAddedEvent evt, VulcanSlowDownNode weapon)
        {
            if (!weapon.vulcanSlowDown.IsAfterShooting)
            {
                AudioSource soundSource = weapon.vulcanSlowDownAfterSpeedUpSoundEffect.SoundSource;
                soundSource.time = weapon.vulcanSlowDownAfterSpeedUpSoundEffect.StartTimePerSec + weapon.vulcanSlowDownAfterSpeedUpSoundEffect.AdditionalStartTimeOffset;
                VulcanFadeSoundBehaviour component = soundSource.gameObject.GetComponent<VulcanFadeSoundBehaviour>();
                component.fadeDuration = weapon.vulcanWeaponState.State * weapon.vulcanWeapon.SlowDownTime;
                component.enabled = true;
                this.PlayNextSound(soundSource, weapon.vulcanSoundManager);
            }
        }

        private void PlaySound(AudioSource sound, VulcanSoundManagerComponent manager)
        {
            sound.PlayDelayed(manager.SoundsWithDelay[sound]);
        }

        [OnEventFire]
        public void PlaySoundAfterShooting(NodeRemoveEvent evt, VulcanAfterShootingSoundNode weapon)
        {
            AudioSource soundSource = weapon.vulcanAfterShootingSoundEffect.SoundSource;
            this.PlayNextSound(soundSource, weapon.vulcanSoundManager);
        }

        [OnEventFire]
        public void PlaySpeedUpSounds(NodeAddedEvent evt, VulcanSpeedUpNode weapon)
        {
            AudioSource soundSource = weapon.vulcanTurbineSoundEffect.SoundSource;
            AudioSource sound = weapon.vulcanChainStartSoundEffect.SoundSource;
            VulcanSoundManagerComponent vulcanSoundManager = weapon.vulcanSoundManager;
            weapon.vulcanSlowDownAfterSpeedUpSoundEffect.AdditionalStartTimeOffset = weapon.vulcanWeapon.SpeedUpTime * (1f - weapon.vulcanWeaponState.State);
            this.PlayNextSound(soundSource, vulcanSoundManager);
            this.PlaySound(sound, vulcanSoundManager);
        }

        private void StopCurrentSound(VulcanSoundManagerComponent manager)
        {
            if (manager.CurrentSound != null)
            {
                manager.CurrentSound.Stop();
            }
        }

        [OnEventFire]
        public void StopCurrentSoundOnIdleState(NodeAddedEvent evt, VulcanIdleStateNode weapon)
        {
            this.StopCurrentSound(weapon.vulcanSoundManager);
        }

        public class VulcanAfterShootingSoundNode : Node
        {
            public VulcanAfterShootingSoundEffectComponent vulcanAfterShootingSoundEffect;
            public WeaponStreamShootingComponent weaponStreamShooting;
            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanAllSoundEffectsNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanAfterShootingSoundEffectComponent vulcanAfterShootingSoundEffect;
            public VulcanChainStartSoundEffectComponent vulcanChainStartSoundEffect;
            public VulcanTurbineSoundEffectComponent vulcanTurbineSoundEffect;
            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class VulcanIdleStateNode : Node
        {
            public VulcanIdleComponent vulcanIdle;
            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanShootingNode : Node
        {
            public WeaponStreamShootingComponent weaponStreamShooting;
            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanSlowDownNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public VulcanSlowDownComponent vulcanSlowDown;
            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;
            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanSpeedUpNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public VulcanSpeedUpComponent vulcanSpeedUp;
            public VulcanSoundManagerComponent vulcanSoundManager;
            public VulcanChainStartSoundEffectComponent vulcanChainStartSoundEffect;
            public VulcanTurbineSoundEffectComponent vulcanTurbineSoundEffect;
            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;
        }
    }
}

