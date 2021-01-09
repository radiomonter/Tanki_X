namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftShotSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitShaftShotSoundEffects(NodeAddedEvent evt, [Combine] InitialShaftShotSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRoot = weapon.weaponSoundRoot.transform;
            weapon.shaftQuickShotSoundEffect.Init(soundRoot);
            weapon.shaftAimingShotSoundEffect.Init(soundRoot);
            weapon.shaftStartCooldownSoundEffect.Init(soundRoot);
            weapon.shaftClosingCooldownSoundEffect.Init(soundRoot);
            ShaftRollCooldownSoundEffectComponent shaftRollCooldownSoundEffect = weapon.shaftRollCooldownSoundEffect;
            shaftRollCooldownSoundEffect.Init(soundRoot);
            shaftRollCooldownSoundEffect.SetFadeOutTime(weapon.shaftShotAnimation.CooldownAnimationTime);
            weapon.Entity.AddComponent<ShaftShotSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayShaftAimingShotEffect(BaseShotEvent evt, ReadyShaftShotSoundEffectNode weapon, [JoinByTank] SingleNode<ShaftAimingWorkFinishStateComponent> state, [JoinByTank] ActiveTankNode tank)
        {
            this.StopCooldownSounds(weapon);
            weapon.shaftQuickShotSoundEffect.Stop();
            weapon.shaftAimingShotSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftClosingCooldownEffect(ShaftShotAnimationCooldownClosingEvent evt, ReadyShaftShotSoundEffectNode weapon)
        {
            weapon.shaftStartCooldownSoundEffect.Stop();
            weapon.shaftClosingCooldownSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftQuickShotEffect(BaseShotEvent evt, ReadyShaftShotSoundEffectNode weapon, [JoinByTank] SingleNode<ShaftIdleStateComponent> state, [JoinByTank] ActiveTankNode tank)
        {
            this.StopCooldownSounds(weapon);
            weapon.shaftAimingShotSoundEffect.Stop();
            weapon.shaftQuickShotSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftStartCooldownEffect(ShaftShotAnimationCooldownStartEvent evt, ReadyShaftShotSoundEffectNode weapon)
        {
            weapon.shaftClosingCooldownSoundEffect.Stop();
            weapon.shaftStartCooldownSoundEffect.Play();
            weapon.shaftRollCooldownSoundEffect.Play();
        }

        private void StopCooldownSounds(ReadyShaftShotSoundEffectNode weapon)
        {
            weapon.shaftStartCooldownSoundEffect.Stop();
            weapon.shaftClosingCooldownSoundEffect.Stop();
            weapon.shaftRollCooldownSoundEffect.Stop();
        }

        [OnEventFire]
        public void StopShaftShotSoundEffects(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyShaftShotSoundEffectNode weapon)
        {
            weapon.shaftAimingShotSoundEffect.Stop();
            weapon.shaftQuickShotSoundEffect.Stop();
            this.StopCooldownSounds(weapon);
        }

        public class ActiveTankNode : Node
        {
            public TankComponent tank;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialShaftShotSoundEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public ShaftQuickShotSoundEffectComponent shaftQuickShotSoundEffect;
            public ShaftAimingShotSoundEffectComponent shaftAimingShotSoundEffect;
            public ShaftStartCooldownSoundEffectComponent shaftStartCooldownSoundEffect;
            public ShaftClosingCooldownSoundEffectComponent shaftClosingCooldownSoundEffect;
            public ShaftRollCooldownSoundEffectComponent shaftRollCooldownSoundEffect;
            public ShaftShotAnimationComponent shaftShotAnimation;
            public ShaftShotAnimationESMComponent shaftShotAnimationEsm;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class ReadyShaftShotSoundEffectNode : ShaftShotSoundEffectSystem.InitialShaftShotSoundEffectNode
        {
            public ShaftShotSoundEffectReadyComponent shaftShotSoundEffectReady;
        }
    }
}

