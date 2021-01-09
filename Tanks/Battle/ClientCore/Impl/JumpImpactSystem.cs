namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class JumpImpactSystem : ECSSystem
    {
        private const float HUNTER_BODY_MASS = 3000f;
        private const float NORMAL_GRAVITY_FORCE = 9.8f;

        [OnEventFire]
        public void EffectAdded(NodeAddedEvent e, JumpEffectNode node, [JoinByTank] TankNode tank)
        {
            GameObject jumpImpactEffect = tank.moduleVisualEffectObjects.JumpImpactEffect;
            if (!jumpImpactEffect.activeInHierarchy)
            {
                jumpImpactEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                jumpImpactEffect.SetActive(true);
            }
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            float num = node.jumpEffect.BaseImpact * node.jumpEffectConfig.ForceUpgradeMult;
            float num2 = !node.jumpEffect.ScaleByMass ? 1f : (rigidbody.mass / 3000f);
            float num3 = (node.jumpEffect.GravityPenalty * (1f - (Physics.gravity.magnitude / 9.8f))) * num;
            Vector3 vector2 = !node.jumpEffect.AlwaysUp ? ((Vector3) (rigidbody.rotation * Vector3.up)) : Vector3.up;
            ImpactEvent eventInstance = new ImpactEvent {
                LocalHitPoint = rigidbody.centerOfMass,
                Force = (Vector3) ((num2 * (num - num3)) * vector2)
            };
            base.ScheduleEvent(eventInstance, tank);
        }

        public class JumpEffectNode : Node
        {
            public JumpEffectComponent jumpEffect;
            public JumpEffectConfigComponent jumpEffectConfig;
        }

        public class TankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;
        }
    }
}

