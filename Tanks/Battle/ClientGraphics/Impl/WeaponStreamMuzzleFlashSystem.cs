namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponStreamMuzzleFlashSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, WeaponStreamMuzzleFlashInitNode node)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(node.weaponStreamMuzzleFlash.EffectPrefab);
            UnityUtil.InheritAndEmplace(obj2.transform, node.muzzlePoint.Current);
            node.weaponStreamMuzzleFlash.EffectInstance = obj2.GetComponent<ParticleSystem>();
            node.weaponStreamMuzzleFlash.LightInstance = obj2.GetComponent<Light>();
            node.Entity.AddComponent<WeaponStreamMuzzleFlashReadyComponent>();
        }

        [OnEventFire]
        public void StartEffect(NodeAddedEvent evt, WeaponStreamMuzzleFlashNode node)
        {
            node.weaponStreamMuzzleFlash.EffectInstance.Play(true);
            node.weaponStreamMuzzleFlash.LightInstance.enabled = true;
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, WeaponStreamMuzzleFlashNode node)
        {
            node.weaponStreamMuzzleFlash.EffectInstance.Stop(true);
            node.weaponStreamMuzzleFlash.LightInstance.enabled = false;
        }

        public class WeaponStreamMuzzleFlashInitNode : Node
        {
            public WeaponStreamMuzzleFlashComponent weaponStreamMuzzleFlash;
            public MuzzlePointComponent muzzlePoint;
        }

        public class WeaponStreamMuzzleFlashNode : Node
        {
            public WeaponStreamMuzzleFlashReadyComponent weaponStreamMuzzleFlashReady;
            public WeaponStreamMuzzleFlashComponent weaponStreamMuzzleFlash;
            public WeaponStreamShootingComponent weaponStreamShooting;
        }
    }
}

