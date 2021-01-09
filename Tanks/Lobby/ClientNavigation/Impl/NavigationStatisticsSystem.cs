namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class NavigationStatisticsSystem : ECSSystem
    {
        [OnEventFire]
        public void SendEnterScreen(NodeAddedEvent e, ActiveScreenNode screen, [JoinAll] ClientSessionNode clientSession)
        {
            base.ScheduleEvent(new EnterScreenEvent(screen.screen.gameObject.name), clientSession);
        }

        public class ActiveScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }

        public class ClientSessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }
    }
}

