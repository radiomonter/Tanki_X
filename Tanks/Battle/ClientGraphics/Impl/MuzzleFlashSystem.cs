namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class MuzzleFlashSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateMuzzleFlash(BaseShotEvent evt, MuzzleFlashNode muzzle)
        {
            this.InstantiateMuzzleEffect(muzzle.muzzleFlash.muzzleFlashPrefab, muzzle, muzzle.muzzleFlash.duration);
        }

        [OnEventFire]
        public void CreateShaftMuzzleFlashOnAnyRemoteShot(RemoteShotEvent evt, ShaftMuzzleFlashNode muzzle)
        {
            this.InstantiateMuzzleEffect(muzzle.shaftMuzzleFlash.muzzleFlashPrefab, muzzle, muzzle.shaftMuzzleFlash.duration);
        }

        [OnEventFire]
        public void CreateShaftMuzzleFlashOnSelfQuickShot(ShotPrepareEvent evt, ShaftMuzzleFlashNode muzzle)
        {
            this.InstantiateMuzzleEffect(muzzle.shaftMuzzleFlash.muzzleFlashPrefab, muzzle, muzzle.shaftMuzzleFlash.duration);
        }

        private void InstantiateMuzzleEffect(GameObject prefab, MuzzlePointNode muzzlePointNode, float duration)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = prefab,
                AutoRecycleTime = duration
            };
            base.ScheduleEvent(eventInstance, muzzlePointNode);
            GameObject gameObject = eventInstance.Instance.gameObject;
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzlePointNode.muzzlePoint.Current);
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
            gameObject.gameObject.SetActive(true);
        }

        public class MuzzleFlashNode : MuzzleFlashSystem.MuzzlePointNode
        {
            public MuzzleFlashComponent muzzleFlash;
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class MuzzlePointNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
        }

        public class ShaftMuzzleFlashNode : MuzzleFlashSystem.MuzzlePointNode
        {
            public ShaftMuzzleFlashComponent shaftMuzzleFlash;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

