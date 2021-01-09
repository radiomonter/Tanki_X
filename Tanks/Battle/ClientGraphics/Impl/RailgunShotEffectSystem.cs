namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class RailgunShotEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void Build(NodeAddedEvent evt, [Combine] RailgunSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(weapon.railgunShotEffect.Asset);
            obj2.transform.parent = weapon.weaponSoundRoot.gameObject.transform;
            obj2.transform.localPosition = Vector3.zero;
            AudioSource component = obj2.GetComponent<AudioSource>();
            weapon.railgunShotEffect.AudioSurce = component;
            weapon.Entity.AddComponent<RailgunShotEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayShotEffect(BaseRailgunChargingShotEvent evt, RailgunSoundEffectReadyNode weapon, [JoinByTank] TankActiveNode tank)
        {
            weapon.railgunShotEffect.AudioSurce.Play();
        }

        [OnEventFire]
        public void StopSound(NodeRemoveEvent evt, TankActiveNode tank, [JoinByTank] RailgunSoundEffectReadyNode weapon)
        {
            AudioSource audioSurce = weapon.railgunShotEffect.AudioSurce;
            if (audioSurce.isPlaying)
            {
                audioSurce.Stop();
            }
        }

        public class RailgunSoundEffectNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public RailgunShotEffectComponent railgunShotEffect;
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }

        public class RailgunSoundEffectReadyNode : RailgunShotEffectSystem.RailgunSoundEffectNode
        {
            public RailgunShotEffectReadyComponent railgunShotEffectReady;
        }

        public class TankActiveNode : Node
        {
            public TankActiveStateComponent tankActiveState;
        }
    }
}

