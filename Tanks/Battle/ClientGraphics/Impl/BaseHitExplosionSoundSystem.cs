namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using UnityEngine;

    public abstract class BaseHitExplosionSoundSystem : ECSSystem
    {
        protected BaseHitExplosionSoundSystem()
        {
        }

        protected void CreateHitExplosionSoundEffect(Vector3 position, GameObject prefab, float duration)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = prefab,
                AutoRecycleTime = duration
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            eventInstance.Instance.position = position;
            eventInstance.Instance.rotation = Quaternion.identity;
            eventInstance.Instance.gameObject.SetActive(true);
        }
    }
}

