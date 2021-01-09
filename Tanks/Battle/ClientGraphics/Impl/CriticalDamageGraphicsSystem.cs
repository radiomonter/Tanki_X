namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class CriticalDamageGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateEffect(CriticalEffectEvent evt, SingleNode<TankVisualRootComponent> tank, [JoinByTank] TankIncarnationWithoutCriticalEffectNode tankIncarnation)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = evt.EffectPrefab,
                AutoRecycleTime = evt.EffectPrefab.GetComponent<ParticleSystem>().duration
            };
            base.ScheduleEvent(eventInstance, tank);
            Transform instance = eventInstance.Instance;
            instance.parent = tank.component.transform;
            instance.localPosition = evt.LocalPosition;
            instance.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void ReceiveCriticalEvent(CriticalDamageEvent evt, SingleNode<CriticalEffectComponent> node)
        {
            CriticalEffectEvent eventInstance = new CriticalEffectEvent {
                EffectPrefab = node.component.EffectAsset,
                LocalPosition = evt.LocalPosition
            };
            base.ScheduleEvent(eventInstance, evt.Target);
        }

        public class TankIncarnationWithoutCriticalEffectNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }
    }
}

