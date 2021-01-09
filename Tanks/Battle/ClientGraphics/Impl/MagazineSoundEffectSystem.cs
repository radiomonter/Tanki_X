namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class MagazineSoundEffectSystem : ECSSystem
    {
        private void InitMagazineSound(InitialMagazineSoundEffectNode weapon, bool selfTank)
        {
            Transform root = weapon.weaponSoundRoot.gameObject.transform;
            this.PrepareMagazineSoundEffect(weapon.magazineLastCartridgeChargeEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineBlowOffEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineOffsetEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineRollEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineCartridgeClickEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineShotEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineBounceEffect, root);
            this.PrepareMagazineSoundEffect(weapon.magazineCooldownEffect, root);
            MagazineShotEffectAudioGroupBehaviour component = weapon.magazineShotEffect.AudioSource.GetComponent<MagazineShotEffectAudioGroupBehaviour>();
            weapon.magazineShotEffect.AudioSource.outputAudioMixerGroup = !selfTank ? component.RemoteShotGroup : component.SelfShotGroup;
            weapon.Entity.AddComponent<MagazineSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void InitMagazineSounds(NodeAddedEvent evt, [Combine] InitialMagazineSoundEffectNode weapon, [Combine, Context, JoinByTank] RemoteTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.InitMagazineSound(weapon, false);
        }

        [OnEventFire]
        public void InitMagazineSounds(NodeAddedEvent evt, InitialMagazineSoundEffectNode weapon, [Context, JoinByTank] SelfTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.InitMagazineSound(weapon, true);
        }

        private AudioSource InstantiateAudioEffect(GameObject prefab, Transform root)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(prefab);
            obj2.transform.parent = root;
            obj2.transform.localPosition = Vector3.zero;
            return obj2.GetComponent<AudioSource>();
        }

        [OnEventFire]
        public void PlayBlowOffEffect(HammerBlowOffEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineBlowOffEffect);
        }

        [OnEventFire]
        public void PlayBounceEffect(HammerBounceEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineBounceEffect);
        }

        [OnEventFire]
        public void PlayClickEffect(HammerCartridgeClickEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineCartridgeClickEffect);
        }

        [OnEventFire]
        public void PlayCooldownEffect(HammerCooldownEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineCooldownEffect);
        }

        [OnEventFire]
        public void PlayLastCartridgeChargeEffect(HammerChargeLastCartridgeEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineLastCartridgeChargeEffect);
        }

        [OnEventFire]
        public void PlayOffsetEffect(HammerOffsetEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineOffsetEffect);
        }

        [OnEventFire]
        public void PlayRollEffect(HammerRollEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineRollEffect);
        }

        [OnEventFire]
        public void PlayShotEffect(HammerMagazineShotEvent evt, ReadyMagazineSoundEffectNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            this.PlaySoundEffect(weapon.magazineShotEffect);
        }

        private void PlaySoundEffect(MagazineSoundEffectComponent soundEffect)
        {
            AudioSource audioSource = soundEffect.AudioSource;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }

        private void PrepareMagazineSoundEffect(MagazineSoundEffectComponent magazineSoundEffect, Transform root)
        {
            GameObject asset = magazineSoundEffect.Asset;
            AudioSource source = this.InstantiateAudioEffect(asset, root);
            magazineSoundEffect.AudioSource = source;
        }

        private void StopAllSounds(ReadyMagazineSoundEffectNode weapon)
        {
            this.StopPlaying(weapon.magazineLastCartridgeChargeEffect);
            this.StopPlaying(weapon.magazineBlowOffEffect);
            this.StopPlaying(weapon.magazineOffsetEffect);
            this.StopPlaying(weapon.magazineRollEffect);
            this.StopPlaying(weapon.magazineCartridgeClickEffect);
            this.StopPlaying(weapon.magazineShotEffect);
            this.StopPlaying(weapon.magazineBounceEffect);
            this.StopPlaying(weapon.magazineCooldownEffect);
        }

        private void StopPlaying(MagazineSoundEffectComponent soundEffect)
        {
            if (soundEffect.AudioSource.isPlaying)
            {
                soundEffect.AudioSource.Stop();
            }
        }

        [OnEventFire]
        public void StopSoundPlay(ExecuteEnergyInjectionEvent evt, ReadyMagazineSoundEffectNode weapon)
        {
            this.StopAllSounds(weapon);
        }

        [OnEventFire]
        public void StopSoundPlay(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyMagazineSoundEffectNode weapon)
        {
            this.StopAllSounds(weapon);
        }

        public class ActiveTankNode : MagazineSoundEffectSystem.TankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class InitialMagazineSoundEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public MagazineWeaponComponent magazineWeapon;
            public WeaponCooldownComponent weaponCooldown;
            public MagazineStorageComponent magazineStorage;
            public WeaponSoundRootComponent weaponSoundRoot;
            public MagazineLastCartridgeChargeEffectComponent magazineLastCartridgeChargeEffect;
            public MagazineBlowOffEffectComponent magazineBlowOffEffect;
            public MagazineOffsetEffectComponent magazineOffsetEffect;
            public MagazineRollEffectComponent magazineRollEffect;
            public MagazineCartridgeClickEffectComponent magazineCartridgeClickEffect;
            public MagazineShotEffectComponent magazineShotEffect;
            public MagazineBounceEffectComponent magazineBounceEffect;
            public MagazineCooldownEffectComponent magazineCooldownEffect;
            public HammerShotAnimationComponent hammerShotAnimation;
            public HammerShotAnimationReadyComponent hammerShotAnimationReady;
            public TankGroupComponent tankGroup;
        }

        public class ReadyMagazineSoundEffectNode : MagazineSoundEffectSystem.InitialMagazineSoundEffectNode
        {
            public MagazineSoundEffectReadyComponent magazineSoundEffectReady;
        }

        public class RemoteTankNode : MagazineSoundEffectSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : MagazineSoundEffectSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}

