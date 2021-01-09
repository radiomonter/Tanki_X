namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class AcceleratedGearsEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, [Combine] EffectReadyNode effect, [JoinByTank, Context] TankNode tank)
        {
            if (!tank.Entity.HasComponent<AcceleratedGearsInstanceComponent>())
            {
                GameObject original = mapEffect.component.PreloadedEffects["acceleratedgears"];
                if (original)
                {
                    GameObject instance = Object.Instantiate<GameObject>(original);
                    instance.SetActive(true);
                    instance.transform.SetParent(tank.mountPoint.MountPoint, false);
                    instance.transform.localPosition = Vector3.zero;
                    tank.Entity.AddComponent(new AcceleratedGearsInstanceComponent(instance));
                }
            }
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent e, EffectReadyNode effect, [JoinByTank] TankWithGearsNode tank)
        {
            tank.acceleratedGearsInstance.Instance.SetActive(false);
        }

        [OnEventFire]
        public void UpdateEffect(TimeUpdateEvent e, TankWithGearsNode tank, [JoinByTank] WeaponNode weapon, [JoinByTank] EffectReadyNode effect)
        {
            tank.acceleratedGearsInstance.Instance.SetActive((weapon.weaponRotationControl.Speed > weapon.weaponRotation.BaseSpeed) && (weapon.weaponRotationControl.EffectiveControl != 0f));
        }

        public class EffectReadyNode : Node
        {
            public AcceleratedGearsEffectComponent acceleratedGearsEffect;
            public TankGroupComponent tankGroup;
        }

        [Not(typeof(AcceleratedGearsInstanceComponent))]
        public class TankNode : Node
        {
            public MountPointComponent mountPoint;
            public TankGroupComponent tankGroup;
        }

        public class TankWithGearsNode : Node
        {
            public AcceleratedGearsInstanceComponent acceleratedGearsInstance;
        }

        public class WeaponNode : Node
        {
            public WeaponRotationComponent weaponRotation;
            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}

