namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ModuleVisualEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitEffects(InitEffectsEvent e, TankInstanceNode tank)
        {
            Transform parent = new GameObject("ModuleVisualEffects").transform;
            parent.SetParent(tank.tankCommonInstance.TankCommonInstance.transform);
            parent.localPosition = Vector3.zero;
            GameObject obj2 = Object.Instantiate<GameObject>(tank.moduleVisualEffectPrefabs.JumpImpactEffectPrefab, parent);
            obj2.SetActive(false);
            GameObject obj3 = Object.Instantiate<GameObject>(tank.moduleVisualEffectPrefabs.ExternalImpactEffectPrefab, parent);
            obj3.SetActive(false);
            GameObject obj4 = Object.Instantiate<GameObject>(tank.moduleVisualEffectPrefabs.FireRingEffectPrefab, parent);
            obj4.SetActive(false);
            GameObject obj5 = Object.Instantiate<GameObject>(tank.moduleVisualEffectPrefabs.ExplosiveMassEffectPrefab, parent);
            obj5.SetActive(false);
            ModuleVisualEffectObjectsComponent component = new ModuleVisualEffectObjectsComponent {
                JumpImpactEffect = obj2,
                ExternalImpactEffect = obj3,
                FireRingEffect = obj4,
                ExplosiveMassEffect = obj5
            };
            tank.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void ScheduleInitEffects(NodeAddedEvent e, TankInstanceNode tank)
        {
            base.NewEvent<InitEffectsEvent>().Attach(tank).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void TurnOffEffectsOnDeath(NodeRemoveEvent e, TankNode tank)
        {
            tank.moduleVisualEffectObjects.JumpImpactEffect.SetActive(false);
            tank.moduleVisualEffectObjects.ExternalImpactEffect.SetActive(false);
            tank.moduleVisualEffectObjects.FireRingEffect.SetActive(false);
            tank.moduleVisualEffectObjects.ExplosiveMassEffect.SetActive(false);
        }

        public class InitEffectsEvent : Event
        {
        }

        public class TankInstanceNode : Node
        {
            public TankCommonInstanceComponent tankCommonInstance;
            public ModuleVisualEffectPrefabsComponent moduleVisualEffectPrefabs;
        }

        public class TankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
            public BattleGroupComponent battleGroup;
            public RigidbodyComponent rigidbody;
            public BaseRendererComponent baseRenderer;
            public TankCollidersComponent tankColliders;
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;
        }
    }
}

