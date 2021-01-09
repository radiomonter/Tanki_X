namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientHUD.API;

    public class ServiceMessageSystem : ECSSystem
    {
        private const string VISIBLE_PROPERTY = "Visible";

        [OnEventFire]
        public void AddESMComponent(NodeAddedEvent e, SingleNode<ServiceMessageComponent> node)
        {
            ServiceMessageESMComponent component = new ServiceMessageESMComponent();
            EntityStateMachine esm = component.Esm;
            esm.AddState<ServiceMessageStates.ServiceMessageHiddenState>();
            esm.AddState<ServiceMessageStates.ServiceMessageVisibleState>();
            node.Entity.AddComponent(component);
            esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void HideServiceMessage(NodeAddedEvent e, ServiceMessageHiddenNode node)
        {
            node.serviceMessage.animator.SetBool("Visible", false);
        }

        [OnEventComplete]
        public void HideServiceMessageViaEvent(HideServiceMessageEvent e, VisibleServiceMessageWithESMNode serviceMessageNode)
        {
            serviceMessageNode.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void ShowServiceMessage(NodeAddedEvent e, ServiceMessageVisibleNode node)
        {
            node.serviceMessage.animator.SetBool("Visible", true);
        }

        public class ServiceMessageHiddenNode : Node
        {
            public ServiceMessageHiddenStateComponent serviceMessageHiddenState;
            public ServiceMessageComponent serviceMessage;
        }

        public class ServiceMessageVisibleNode : Node
        {
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
            public ServiceMessageComponent serviceMessage;
        }

        public class VisibleServiceMessageWithESMNode : Node
        {
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
            public ServiceMessageComponent serviceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
        }
    }
}

