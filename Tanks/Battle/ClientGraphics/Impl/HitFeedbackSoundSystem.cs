namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class HitFeedbackSoundSystem : ECSSystem
    {
        private SoundController AttachStreamSoundController(SoundController asset, Transform root) => 
            Object.Instantiate<SoundController>(asset, root.position, root.rotation, root);

        [OnEventFire]
        public void ClearHitFeedbackSounds(KillTankSoundEffectCreatedEvent e, SingleNode<SoundListenerComponent> listener)
        {
            WeaponFeedbackSoundBehaviour.ClearHitFeedbackSounds();
        }

        [OnEventFire]
        public void InitFlamethrowerFeedbackSounds(NodeAddedEvent e, FlamethrowerWeaponNode weapon, [Context, JoinByTank] SelfTankNode tank)
        {
            this.InitStreamWeaponHitFeedback(weapon, tank, tank.hitFeedbackSounds.FlamethrowerWeaponAttackController);
        }

        [OnEventFire]
        public void InitFreezeFeedbackSounds(NodeAddedEvent e, FreezeWeaponNode weapon, [Context, JoinByTank] SelfTankNode tank)
        {
            this.InitStreamWeaponHitFeedback(weapon, tank, tank.hitFeedbackSounds.FreezeWeaponAttackController);
        }

        [OnEventFire]
        public void InitIsisFeedbackSounds(NodeAddedEvent e, IsisWeaponNode isis, [Context, JoinByTank] SelfTankNode tank)
        {
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            SoundController isisAttackFeedbackController = tank.hitFeedbackSounds.IsisAttackFeedbackController;
            SoundController isisHealingFeedbackController = tank.hitFeedbackSounds.IsisHealingFeedbackController;
            isis.Entity.AddComponent(new IsisHitFeedbackReadyComponent(this.AttachStreamSoundController(isisHealingFeedbackController, soundRootTransform), this.AttachStreamSoundController(isisAttackFeedbackController, soundRootTransform)));
        }

        private void InitStreamWeaponHitFeedback(WeaponNode weapon, SelfTankNode tank, SoundController attackAsset)
        {
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            weapon.Entity.AddComponent(new StreamWeaponHitFeedbackReadyComponent(this.AttachStreamSoundController(attackAsset, soundRootTransform)));
        }

        private void PlayHitFeedbackSound(WeaponFeedbackSoundBehaviour asset, HitFeedbackSoundsPlayingSettingsComponent settings)
        {
            Object.Instantiate<WeaponFeedbackSoundBehaviour>(asset).Play(settings.Delay, settings.Volume, settings.RemoveOnKillSound);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] HammerHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.HammerHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] RailgunHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.RailgunHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] RicochetHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.RicochetHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] ShaftHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.ShaftHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] SmokyHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.SmokyHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] ThunderHitFeedbackSoundNode weapon, [JoinAll] SoundListenerNode listener)
        {
            this.PlayHitFeedbackSound(tank.component.ThunderHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void StartIsisAttackFeedbackSound(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [JoinSelf, Context] IsisReadyWeaponNode isis, SoundListenerNode listener)
        {
            isis.isisHitFeedbackReady.AttackSoundController.FadeIn();
        }

        [OnEventFire]
        public void StartIsisHealingFeedbackSound(NodeAddedEvent e, SingleNode<StreamHitTeammateFeedbackComponent> weapon, [JoinSelf, Context] IsisReadyWeaponNode isis, SoundListenerNode listener)
        {
            isis.isisHitFeedbackReady.HealingSoundController.FadeIn();
        }

        [OnEventFire]
        public void StartStreamHitFeedbackSounds(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> streamHitFeedback, [JoinSelf, Context] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon)
        {
            weapon.component.SoundController.FadeIn();
        }

        [OnEventFire]
        public void StopIsisAttackFeedbackSound(NodeRemoveEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [JoinSelf] IsisReadyWeaponNode isis)
        {
            isis.isisHitFeedbackReady.AttackSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopIsisFeedbackSounds(NodeRemoveEvent e, SingleNode<SoundListenerReadyForHitFeedbackComponent> listener, [JoinAll] IsisReadyWeaponNode isis)
        {
            isis.isisHitFeedbackReady.HealingSoundController.FadeOut();
            isis.isisHitFeedbackReady.AttackSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopIsisHealingFeedbackSound(NodeRemoveEvent e, SingleNode<StreamHitTeammateFeedbackComponent> weapon, [JoinSelf] IsisReadyWeaponNode isis)
        {
            isis.isisHitFeedbackReady.HealingSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopStreamHitFeedbackSounds(NodeRemoveEvent e, SingleNode<StreamHitEnemyFeedbackComponent> streamHitFeedback, [JoinSelf] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon)
        {
            weapon.component.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StopStreamHitFeedbackSounds(NodeRemoveEvent e, SingleNode<SoundListenerReadyForHitFeedbackComponent> listener, [JoinAll] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon)
        {
            weapon.component.SoundController.FadeOut();
        }

        public class FlamethrowerWeaponNode : HitFeedbackSoundSystem.WeaponNode
        {
            public FlamethrowerComponent flamethrower;
        }

        public class FreezeWeaponNode : HitFeedbackSoundSystem.WeaponNode
        {
            public FreezeComponent freeze;
        }

        public class HammerHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public HammerComponent hammer;
        }

        public class HitFeedbackSoundsPlayingSettingsWeaponNode : HitFeedbackSoundSystem.WeaponNode
        {
            public HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings;
        }

        public class IsisReadyWeaponNode : HitFeedbackSoundSystem.IsisWeaponNode
        {
            public IsisHitFeedbackReadyComponent isisHitFeedbackReady;
        }

        public class IsisWeaponNode : HitFeedbackSoundSystem.WeaponNode
        {
            public IsisComponent isis;
        }

        public class RailgunHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public RailgunComponent railgun;
        }

        public class RicochetHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public RicochetComponent ricochet;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankSoundRootComponent tankSoundRoot;
            public HitFeedbackSoundsComponent hitFeedbackSounds;
            public TankGroupComponent tankGroup;
        }

        public class ShaftHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public ShaftComponent shaft;
        }

        public class SmokyHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public SmokyComponent smoky;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerBattleStateComponent soundListenerBattleState;
            public SoundListenerReadyForHitFeedbackComponent soundListenerReadyForHitFeedback;
        }

        public class ThunderHitFeedbackSoundNode : HitFeedbackSoundSystem.HitFeedbackSoundsPlayingSettingsWeaponNode
        {
            public ThunderComponent thunder;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }
    }
}

