namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankExplosionGraphicsSystem : ECSSystem
    {
        private void PlayEffect(ParticleSystem prefab, Transform visualRoot, Transform mountPoint, float timeToPlay, Node entity)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = prefab.gameObject,
                AutoRecycleTime = Mathf.Min(timeToPlay, prefab.main.duration)
            };
            base.ScheduleEvent(eventInstance, entity);
            Transform instance = eventInstance.Instance;
            GameObject gameObject = instance.gameObject;
            instance.parent = visualRoot;
            instance.localPosition = mountPoint.localPosition;
            instance.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            gameObject.GetComponent<ParticleSystem>().Play(true);
        }

        [OnEventFire]
        public void ShowExplosion(NodeAddedEvent evt, TankNode tank)
        {
            if (tank.cameraVisibleTrigger.IsVisible)
            {
                float timeToPlay = tank.tankDeadState.EndDate.UnityTime - Time.time;
                this.PlayEffect(tank.tankDeathExplosionPrefabs.ExplosionPrefab, tank.tankVisualRoot.transform, tank.mountPoint.MountPoint, timeToPlay, tank);
            }
        }

        [OnEventFire]
        public void ShowFire(NodeAddedEvent evt, TankNode tank, [JoinByTank] NormalTurretNode turret)
        {
            if (tank.cameraVisibleTrigger.IsVisible)
            {
                float timeToPlay = (float) (tank.tankDeadState.EndDate - Date.Now);
                this.PlayEffect(tank.tankDeathExplosionPrefabs.FirePrefab, tank.tankVisualRoot.transform, tank.mountPoint.MountPoint, timeToPlay, tank);
            }
        }

        [Not(typeof(WeaponUndergroundComponent))]
        public class NormalTurretNode : Node
        {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class TankNode : Node
        {
            public TankDeadStateComponent tankDeadState;
            public TankCommonInstanceComponent tankCommonInstance;
            public TankDeathExplosionPrefabsComponent tankDeathExplosionPrefabs;
            public MountPointComponent mountPoint;
            public TankVisualRootComponent tankVisualRoot;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
        }
    }
}

