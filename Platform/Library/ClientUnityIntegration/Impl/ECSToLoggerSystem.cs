namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class ECSToLoggerSystem : ECSSystem
    {
        [OnEventFire]
        public void RegisterSession(NodeAddedEvent e, SingleNode<ClientSessionComponent> session)
        {
            ECStoLoggerGateway.ClientSessionId = session.Entity.Id;
        }

        [OnEventFire]
        public void UnRegisterSession(NodeRemoveEvent e, SingleNode<ClientSessionComponent> session)
        {
            ECStoLoggerGateway.ClientSessionId = 0L;
        }
    }
}

