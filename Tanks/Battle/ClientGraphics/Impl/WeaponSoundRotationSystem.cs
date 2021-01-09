namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class WeaponSoundRotationSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateWeaponRotationSound(NodeAddedEvent evt, [Combine] WeaponSoundNode weaponNode, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            WeaponRotationSoundComponent weaponRotationSound = weaponNode.weaponRotationSound;
            GameObject obj2 = Object.Instantiate<GameObject>(weaponRotationSound.Asset);
            obj2.transform.parent = weaponNode.weaponSoundRoot.gameObject.transform;
            obj2.transform.localPosition = Vector3.zero;
            WeaponRotationSoundBehaviour component = obj2.GetComponent<WeaponRotationSoundBehaviour>();
            weaponRotationSound.StartAudioSource = component.startAudioSource;
            weaponRotationSound.LoopAudioSource = component.loopAudioSource;
            weaponRotationSound.StopAudioSource = component.stopAudioSource;
            weaponRotationSound.IsActive = false;
            weaponNode.Entity.AddComponent<WeaponRotationSoundReadyComponent>();
        }

        private void StartAudioSources(WeaponRotationSoundComponent sounds)
        {
            if (!sounds.IsActive)
            {
                sounds.StopAudioSource.Stop();
                sounds.StartAudioSource.Play();
                double time = AudioSettings.dspTime + sounds.StartAudioSource.clip.length;
                sounds.LoopAudioSource.PlayScheduled(time);
                sounds.IsActive = true;
            }
        }

        private void StopAudioSources(WeaponRotationSoundComponent sounds)
        {
            if (sounds.IsActive)
            {
                sounds.StartAudioSource.Stop();
                sounds.LoopAudioSource.Stop();
                sounds.StopAudioSource.Play();
                sounds.IsActive = false;
            }
        }

        [OnEventFire]
        public void StopWeaponRotationSound(NodeRemoveEvent evt, ReadyWeaponSoundRotationNode weapon)
        {
            this.StopAudioSources(weapon.weaponRotationSound);
        }

        [OnEventFire]
        public void StopWeaponRotationSound(NodeRemoveEvent evt, ActiveTankNode activeTank, [JoinByTank] ReadyWeaponSoundRotationNode weapon)
        {
            this.StopAudioSources(weapon.weaponRotationSound);
        }

        [OnEventComplete]
        public void UpdateWeaponRotationSound(UpdateEvent evt, ReadyWeaponSoundRotationNode weapon, [JoinByTank] ActiveTankNode activeTank)
        {
            if (weapon.weaponRotationControl.IsRotating())
            {
                this.StartAudioSources(weapon.weaponRotationSound);
            }
            else
            {
                this.StopAudioSources(weapon.weaponRotationSound);
            }
        }

        [Not(typeof(TankAutopilotComponent))]
        public class ActiveTankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public TankMovableComponent tankMovable;
        }

        public class ReadyWeaponSoundRotationNode : WeaponSoundRotationSystem.WeaponSoundNode
        {
            public WeaponRotationComponent weaponRotation;
            public WeaponRotationSoundReadyComponent weaponRotationSoundReady;
            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class WeaponSoundNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public TankGroupComponent tankGroup;
            public WeaponSoundRootComponent weaponSoundRoot;
            public WeaponRotationSoundComponent weaponRotationSound;
        }
    }
}

