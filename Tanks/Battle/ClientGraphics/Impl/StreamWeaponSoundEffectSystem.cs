namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class StreamWeaponSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void Build(NodeAddedEvent evt, [Combine] StreamWeaponSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(weapon.streamWeaponSoundEffect.Asset);
            obj2.transform.parent = weapon.weaponSoundRoot.gameObject.transform;
            obj2.transform.localPosition = Vector3.zero;
            weapon.streamWeaponSoundEffect.SoundController = obj2.GetComponent<SoundController>();
            weapon.Entity.AddComponent<StreamWeaponSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartSoundEffect(NodeAddedEvent evt, WorkingNode weapon)
        {
            weapon.streamWeaponSoundEffect.SoundController.FadeIn();
        }

        [OnEventFire]
        public void StopSoundEffect(NodeRemoveEvent evt, WorkingNode weapon)
        {
            weapon.streamWeaponSoundEffect.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StopSoundEffect(NodeRemoveEvent evt, ActivaTankNode tank, [JoinByTank] WorkingNode weapon)
        {
            weapon.streamWeaponSoundEffect.SoundController.FadeOut();
        }

        public class ActivaTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class StreamWeaponSoundEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public StreamWeaponComponent streamWeapon;
            public StreamWeaponSoundEffectComponent streamWeaponSoundEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class WorkingNode : Node
        {
            public StreamWeaponComponent streamWeapon;
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public StreamWeaponSoundEffectComponent streamWeaponSoundEffect;
            public StreamWeaponSoundEffectReadyComponent streamWeaponSoundEffectReady;
        }
    }
}

