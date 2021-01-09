namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class FireRingEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void EnableEffect(NodeAddedEvent e, FireEffectNode effectNode, [JoinByTank] TankNode tank)
        {
            GameObject fireRingEffect = tank.moduleVisualEffectObjects.FireRingEffect;
            if (!fireRingEffect.activeInHierarchy)
            {
                fireRingEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                fireRingEffect.SetActive(true);
            }
            base.ScheduleEvent<StartSplashEffectEvent>(effectNode);
        }

        public class FireEffectNode : Node
        {
            public FireRingEffectComponent fireRingEffect;
        }

        public class TankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;
        }
    }
}

