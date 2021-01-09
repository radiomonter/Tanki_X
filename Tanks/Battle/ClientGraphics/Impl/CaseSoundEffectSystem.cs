namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class CaseSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitCaseEjectionSound(NodeAddedEvent evt, [Combine] InitialCaseEjectionSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform root = weapon.weaponSoundRoot.gameObject.transform;
            this.PrepareCaseSoundEffect(weapon.caseEjectionSoundEffect, root);
            if (!weapon.Entity.HasComponent<CaseEjectionSoundEffectReadyComponent>())
            {
                weapon.Entity.AddComponent<CaseEjectionSoundEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void InitCaseEjectorMovementEffects(NodeAddedEvent evt, [Combine] InitialCaseEjectorMovementSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform root = weapon.weaponSoundRoot.gameObject.transform;
            this.PrepareCaseSoundEffect(weapon.caseEjectorOpeningSoundEffect, root);
            this.PrepareCaseSoundEffect(weapon.caseEjectorClosingSoundEffect, root);
            Entity entity = weapon.Entity;
            weapon.caseEjectorMovementTrigger.Entity = entity;
            if (!entity.HasComponent<CaseEjectorMovementSoundEffectReadyComponent>())
            {
                entity.AddComponent<CaseEjectorMovementSoundEffectReadyComponent>();
            }
        }

        private AudioSource InstantiateCaseSoundEffect(GameObject prefab, Transform root)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(prefab);
            obj2.transform.parent = root;
            obj2.transform.localPosition = Vector3.zero;
            return obj2.GetComponent<AudioSource>();
        }

        [OnEventFire]
        public void PlayCaseEjectionSound(CartridgeCaseEjectionEvent evt, ReadyCaseEjectionSoundEffectNode weapon)
        {
            weapon.caseEjectionSoundEffect.Source.Play();
        }

        [OnEventFire]
        public void PlayEjectorClosingEffect(CaseEjectorCloseEvent evt, ReadyCaseEjectorMovementSoundEffectNode weapon)
        {
            weapon.caseEjectorOpeningSoundEffect.Source.Stop();
            weapon.caseEjectorClosingSoundEffect.Source.Play();
        }

        [OnEventFire]
        public void PlayEjectorOpeningEffect(CaseEjectorOpenEvent evt, ReadyCaseEjectorMovementSoundEffectNode weapon)
        {
            weapon.caseEjectorClosingSoundEffect.Source.Stop();
            weapon.caseEjectorOpeningSoundEffect.Source.Play();
        }

        private void PrepareCaseSoundEffect(CaseSoundEffectComponent caseSoundEffect, Transform root)
        {
            GameObject caseSoundAsset = caseSoundEffect.CaseSoundAsset;
            AudioSource source = this.InstantiateCaseSoundEffect(caseSoundAsset, root);
            caseSoundEffect.Source = source;
        }

        public class InitialCaseEjectionSoundEffectNode : Node
        {
            public CaseEjectionSoundEffectComponent caseEjectionSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public AnimationPreparedComponent animationPrepared;
            public TankGroupComponent tankGroup;
        }

        public class InitialCaseEjectorMovementSoundEffectNode : Node
        {
            public CaseEjectorOpeningSoundEffectComponent caseEjectorOpeningSoundEffect;
            public CaseEjectorClosingSoundEffectComponent caseEjectorClosingSoundEffect;
            public CaseEjectorMovementTriggerComponent caseEjectorMovementTrigger;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class ReadyCaseEjectionSoundEffectNode : Node
        {
            public CaseEjectionSoundEffectComponent caseEjectionSoundEffect;
            public CaseEjectionSoundEffectReadyComponent caseEjectionSoundEffectReady;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class ReadyCaseEjectorMovementSoundEffectNode : Node
        {
            public CaseEjectorOpeningSoundEffectComponent caseEjectorOpeningSoundEffect;
            public CaseEjectorClosingSoundEffectComponent caseEjectorClosingSoundEffect;
            public CaseEjectorMovementSoundEffectReadyComponent caseEjectorMovementSoundEffectReady;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }
    }
}

