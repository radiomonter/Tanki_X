namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;

    public class ConnectionReportSystem : ECSSystem
    {
        public bool hasConnection;

        [OnEventFire]
        public void Set(NodeAddedEvent e, SessionNode session)
        {
            this.hasConnection = true;
        }

        public class SessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }
    }
}

