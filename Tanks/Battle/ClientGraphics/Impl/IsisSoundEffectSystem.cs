namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class IsisSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableIsisTargetSound(NodeRemoveEvent e, TargetEffectNode weapon, [JoinByTank] IsisCurrentSoundEffectNode isisSound)
        {
            IsisCurrentSoundEffectComponent isisCurrentSoundEffect = isisSound.isisCurrentSoundEffect;
            this.UpdateIsisSoundEffect(isisCurrentSoundEffect, weapon.isisHealingSoundEffect.SoundController);
        }

        [OnEventFire]
        public void EnableIsisTargetSound(NodeAddedEvent e, TargetEffectNode weapon, [Context, JoinByTank] IsisCurrentSoundEffectNode isisSound, [Context, JoinByBattle] DMNode dm)
        {
            IsisCurrentSoundEffectComponent isisCurrentSoundEffect = isisSound.isisCurrentSoundEffect;
            this.UpdateIsisSoundEffect(isisCurrentSoundEffect, weapon.isisDamagingSoundEffect.SoundController);
        }

        [OnEventFire]
        public void EnableIsisTargetSound(NodeAddedEvent e, [Combine] TargetEffectTeamNode weapon, [Combine, Context, JoinByTank] IsisCurrentSoundEffectNode isisSound, [Context, JoinByTeam] TeamNode weaponTeam)
        {
            Entity entity = weapon.streamHit.TankHit.Entity;
            base.NewEvent<UpdateIsisSoundModeEvent>().Attach(weapon).Attach(entity).Schedule();
        }

        private void InitIsisSoundEffect(AbstractIsisSoundEffectComponent isisSoundEffect, Transform root)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(isisSoundEffect.Asset);
            obj2.transform.parent = root;
            obj2.transform.localPosition = Vector3.zero;
            isisSoundEffect.SoundController = obj2.GetComponent<SoundController>();
        }

        private void InitIsisSounds(InitialIsisSoundEffectNode weapon)
        {
            Transform root = weapon.weaponSoundRoot.transform;
            this.InitIsisSoundEffect(weapon.isisDamagingSoundEffect, root);
            this.InitIsisSoundEffect(weapon.isisHealingSoundEffect, root);
            weapon.Entity.AddComponent<IsisSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void InitIsisSounds(NodeAddedEvent evt, [Combine] InitialIsisSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.InitIsisSounds(weapon);
        }

        [OnEventFire]
        public void PlayIsisSound(NodeAddedEvent evt, SingleNode<IsisCurrentSoundEffectComponent> weapon)
        {
            this.PlayIsisSoundEffect(weapon.component);
        }

        private void PlayIsisSoundEffect(IsisCurrentSoundEffectComponent currentSoundEffect)
        {
            if (!currentSoundEffect.WasStarted)
            {
                currentSoundEffect.WasStarted = true;
                currentSoundEffect.SoundController.FadeIn();
            }
        }

        [OnEventFire]
        public void StartWorking(NodeAddedEvent evt, ReadyIsisSoundEffectWorkingNode weapon)
        {
            IsisCurrentSoundEffectComponent component = new IsisCurrentSoundEffectComponent {
                SoundController = weapon.isisHealingSoundEffect.SoundController
            };
            weapon.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void StopIsisSound(NodeRemoveEvent evt, SingleNode<IsisCurrentSoundEffectComponent> weapon)
        {
            if (weapon.component.SoundController)
            {
                this.StopIsisSoundEffect(weapon.component);
            }
        }

        private void StopIsisSoundEffect(IsisCurrentSoundEffectComponent currentSoundEffect)
        {
            currentSoundEffect.WasStopped = true;
            currentSoundEffect.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StopWorking(NodeRemoveEvent evt, ReadyIsisSoundEffectWorkingNode weapon)
        {
            if (weapon.Entity.HasComponent<IsisCurrentSoundEffectComponent>())
            {
                weapon.Entity.RemoveComponent<IsisCurrentSoundEffectComponent>();
            }
        }

        private void UpdateIsisSoundEffect(IsisCurrentSoundEffectComponent isisCurrentSoundEffect, SoundController soundController)
        {
            if (!isisCurrentSoundEffect.WasStopped)
            {
                if (!isisCurrentSoundEffect.WasStarted)
                {
                    isisCurrentSoundEffect.SoundController = soundController;
                    isisCurrentSoundEffect.WasStarted = true;
                    soundController.FadeIn();
                }
                else if (isisCurrentSoundEffect.SoundController != soundController)
                {
                    isisCurrentSoundEffect.SoundController.FadeOut();
                    isisCurrentSoundEffect.SoundController = soundController;
                    soundController.FadeIn();
                }
            }
        }

        [OnEventFire]
        public void UpdateIsisSoundMode(UpdateIsisSoundModeEvent evt, IsisCurrentSoundTeamEffectNode weapon, TankTeamNode target)
        {
            SoundController soundController = (weapon.teamGroup.Key != target.teamGroup.Key) ? weapon.isisDamagingSoundEffect.SoundController : weapon.isisHealingSoundEffect.SoundController;
            this.UpdateIsisSoundEffect(weapon.isisCurrentSoundEffect, soundController);
        }

        public class DMNode : Node
        {
            public DMComponent dm;
            public BattleGroupComponent battleGroup;
        }

        public class InitialIsisSoundEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class IsisCurrentSoundEffectNode : Node
        {
            public IsisCurrentSoundEffectComponent isisCurrentSoundEffect;
            public TankGroupComponent tankGroup;
        }

        public class IsisCurrentSoundTeamEffectNode : Node
        {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public IsisSoundEffectReadyComponent isisSoundEffectReady;
            public IsisCurrentSoundEffectComponent isisCurrentSoundEffect;
            public TeamGroupComponent teamGroup;
        }

        public class ReadyIsisSoundEffectNode : Node
        {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public IsisSoundEffectReadyComponent isisSoundEffectReady;
        }

        public class ReadyIsisSoundEffectWorkingNode : Node
        {
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public IsisSoundEffectReadyComponent isisSoundEffectReady;
        }

        public class TankTeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TankComponent tank;
        }

        public class TargetEffectNode : Node
        {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public IsisSoundEffectReadyComponent isisSoundEffectReady;
            public StreamHitComponent streamHit;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
            public BattleGroupComponent battleGroup;
            public TankGroupComponent tankGroup;
        }

        public class TargetEffectTeamNode : Node
        {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;
            public IsisHealingSoundEffectComponent isisHealingSoundEffect;
            public IsisSoundEffectReadyComponent isisSoundEffectReady;
            public StreamHitComponent streamHit;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
            public BattleGroupComponent battleGroup;
            public TankGroupComponent tankGroup;
            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TeamComponent team;
        }
    }
}

