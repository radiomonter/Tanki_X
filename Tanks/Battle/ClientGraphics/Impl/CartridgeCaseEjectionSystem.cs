namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class CartridgeCaseEjectionSystem : ECSSystem
    {
        private const float MAX_WORK_DISTANCE = 30f;

        [OnEventFire]
        public void CreateCaseContainer(NodeAddedEvent e, SingleNode<MapInstanceComponent> node)
        {
            node.Entity.AddComponent(new CartridgeCaseContainerComponent());
        }

        [OnEventFire]
        public void EjectCase(CartridgeCaseEjectionEvent e, SingleNode<CartridgeCaseEjectorComponent> ejectorNode, [JoinByTank] HullNode hullNode, [JoinAll] SingleNode<CartridgeCaseContainerComponent> containerNode)
        {
            if (hullNode.Entity.HasComponent<SelfTankComponent>() || hullNode.cameraVisibleTrigger.IsVisibleAtRange(30f))
            {
                GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                    Prefab = ejectorNode.component.casePrefab
                };
                base.ScheduleEvent(eventInstance, ejectorNode);
                GameObject gameObject = eventInstance.Instance.gameObject;
                this.SetCaseTransform(gameObject, ejectorNode.component);
                this.SetCaseVelocity(gameObject, ejectorNode.component, hullNode);
                gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void RemoveCaseContainer(NodeRemoveEvent e, SingleNode<MapInstanceComponent> node)
        {
            node.Entity.RemoveComponent<CartridgeCaseContainerComponent>();
        }

        private void SetCaseTransform(GameObject cartridgeCase, CartridgeCaseEjectorComponent component)
        {
            cartridgeCase.transform.position = component.transform.TransformPoint(Vector3.zero);
            cartridgeCase.transform.Rotate(component.transform.eulerAngles);
        }

        private void SetCaseVelocity(GameObject cartridgeCase, CartridgeCaseEjectorComponent component, HullNode hullNode)
        {
            GameObject hullInstance = hullNode.hullInstance.HullInstance;
            Rigidbody rigidbody = hullNode.rigidbody.Rigidbody;
            Rigidbody rigidbody2 = cartridgeCase.GetComponent<Rigidbody>();
            Vector3 rhs = cartridgeCase.transform.position - hullInstance.transform.position;
            rigidbody2.SetVelocitySafe((component.transform.TransformDirection((Vector3) (component.initialSpeed * Vector3.forward)) + rigidbody.velocity) + Vector3.Cross(rigidbody.angularVelocity, rhs));
            rigidbody2.SetAngularVelocitySafe(component.transform.TransformDirection((Vector3) (component.initialAngularSpeed * Vector3.up)) + rigidbody.angularVelocity);
        }

        [OnEventFire]
        public void SetupEjectionTrigger(NodeAddedEvent e, SingleNode<CartridgeCaseEjectionTriggerComponent> node)
        {
            node.component.Entity = node.Entity;
        }

        public class HullNode : Node
        {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
            public HullInstanceComponent hullInstance;
            public RigidbodyComponent rigidbody;
        }
    }
}

