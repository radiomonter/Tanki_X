namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShaftAimingSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableWeaponRotationSound(NodeAddedEvent evt, ShaftAimingWorkActivationWeaponRotationSoundNode weapon)
        {
            weapon.Entity.RemoveComponentIfPresent<WeaponRotationSoundReadyComponent>();
            weapon.Entity.AddComponentIfAbsent<ShaftAimingRotationSoundReadyStateComponent>();
        }

        [OnEventFire]
        public void EnableWeaponRotationSound(NodeAddedEvent evt, ShaftIdleWeaponRotationSoundNode weapon)
        {
            weapon.Entity.RemoveComponentIfPresent<ShaftAimingRotationSoundReadyStateComponent>();
            weapon.Entity.AddComponentIfAbsent<WeaponRotationSoundReadyComponent>();
        }

        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForAnyWeapon(NodeAddedEvent evt, [Combine] InitialAnyShaftAimingSoundsEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRoot = weapon.weaponSoundRoot.transform;
            ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect = weapon.shaftStartAimingSoundEffect;
            ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect = weapon.shaftAimingLoopSoundEffect;
            shaftStartAimingSoundEffect.Init(soundRoot);
            shaftAimingLoopSoundEffect.Init(soundRoot);
            shaftAimingLoopSoundEffect.SetDelay(shaftStartAimingSoundEffect.StartAimingDurationSec);
            weapon.Entity.AddComponentIfAbsent<ShaftAnyAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForRemoteWeapon(NodeAddedEvent evt, [Combine] InitialRemoteShaftAimingSoundsEffectNode weapon, [Combine, Context, JoinByTank] RemoteTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRoot = weapon.weaponSoundRoot.transform;
            weapon.shaftAimingOptixMovementSoundEffect.Init(soundRoot);
            weapon.Entity.AddComponentIfAbsent<ShaftRemoteAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForSelfWeapon(NodeAddedEvent evt, [Combine] InitialSelfShaftAimingSoundsEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRoot = weapon.weaponSoundRoot.transform;
            weapon.shaftAimingControllerSoundEffect.Init(soundRoot);
            weapon.Entity.AddComponentIfAbsent<ShaftSelfAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void PlayAimingSounds(NodeAddedEvent evt, ShaftStartLoopAimingSoundsWorkActivationNode weapon)
        {
            weapon.shaftStartAimingSoundEffect.Play();
            weapon.shaftAimingLoopSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayTargetingControllerSound(UpdateEvent evt, ShaftAimingControllerSoundEffectPlayingWorkingStateNode weapon)
        {
            if (weapon.shaftAimingWorkingState.IsActive)
            {
                weapon.shaftAimingControllerSoundEffect.Play();
            }
            else
            {
                weapon.shaftAimingControllerSoundEffect.Stop();
            }
        }

        [OnEventFire]
        public void StartOptixMovementEffect(NodeAddedEvent evt, ShaftAimingWorkingStateSoundEffectNode weapon)
        {
            if (weapon.shaftAimingOptixMovementSoundEffect != null)
            {
                weapon.shaftAimingOptixMovementSoundEffect.Play();
            }
        }

        [OnEventFire]
        public void StartTargetingControllerSound(NodeAddedEvent evt, ShaftAimingControllerSoundEffectWorkingNode weapon)
        {
            weapon.Entity.AddComponentIfAbsent<ShaftAimingControllerPlayingComponent>();
        }

        [OnEventFire]
        public void StopAimingSounds(NodeAddedEvent evt, ShaftStartLoopAimingSoundsIdleNode weapon)
        {
            weapon.shaftStartAimingSoundEffect.Stop();
            weapon.shaftAimingLoopSoundEffect.Stop();
        }

        [OnEventFire]
        public void StopAimingSounds(NodeRemoveEvent evt, ShaftStartLoopAimingSoundsWorkingNode weapon)
        {
            weapon.shaftStartAimingSoundEffect.Stop();
            weapon.shaftAimingLoopSoundEffect.Stop();
        }

        [OnEventFire]
        public void StopOptixMovementEffect(NodeRemoveEvent evt, ShaftAimingWorkingStateSoundEffectNode weapon)
        {
            if (weapon.shaftAimingOptixMovementSoundEffect != null)
            {
                weapon.shaftAimingOptixMovementSoundEffect.Stop();
            }
        }

        [OnEventFire]
        public void StopPlayingTargetingControllerSound(NodeAddedEvent evt, ShaftAimingControllerSoundEffectIdleNode weapon)
        {
            weapon.Entity.RemoveComponentIfPresent<ShaftAimingControllerPlayingComponent>();
        }

        [OnEventFire]
        public void StopPlayingTargetingControllerSound(NodeRemoveEvent evt, ShaftAimingControllerSoundEffectPlayingNode weapon)
        {
            weapon.shaftAimingControllerSoundEffect.Stop();
        }

        public class InitialAnyShaftAimingSoundsEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class InitialRemoteShaftAimingSoundsEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public ShaftAimingOptixMovementSoundEffectComponent shaftAimingOptixMovementSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class InitialSelfShaftAimingSoundsEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;
            public ShaftStateControllerComponent shaftStateController;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class RemoteTankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingControllerSoundEffectIdleNode : Node
        {
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;
            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftAimingControllerSoundEffectPlayingNode : Node
        {
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;
            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
        }

        public class ShaftAimingControllerSoundEffectPlayingWorkingStateNode : Node
        {
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;
            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public WeaponRotationComponent weaponRotation;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
        }

        public class ShaftAimingControllerSoundEffectWorkingNode : Node
        {
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;
            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ShaftAimingWorkActivationWeaponRotationSoundNode : Node
        {
            public WeaponRotationSoundReadyComponent weaponRotationSoundReady;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
        }

        public class ShaftAimingWorkingStateSoundEffectNode : Node
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingOptixMovementSoundEffectComponent shaftAimingOptixMovementSoundEffect;
            public ShaftRemoteAimingSoundsInstantiatedComponent shaftRemoteAimingSoundsInstantiated;
        }

        public class ShaftIdleWeaponRotationSoundNode : Node
        {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftAimingRotationSoundReadyStateComponent shaftAimingRotationSoundReadyState;
        }

        public class ShaftStartLoopAimingSoundsIdleNode : Node
        {
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;
            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftStartLoopAimingSoundsWorkActivationNode : Node
        {
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;
            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
        }

        public class ShaftStartLoopAimingSoundsWorkingNode : Node
        {
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;
            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }
    }
}

