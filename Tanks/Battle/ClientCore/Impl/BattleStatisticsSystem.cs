namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;

    public class BattleStatisticsSystem : ECSSystem
    {
        private static bool firstLoad = true;

        [OnEventFire]
        public void LogFirstGarageEntrance(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, [JoinAll] SessionNode session)
        {
            if (firstLoad)
            {
                firstLoad = false;
                base.ScheduleEvent<ClientGarageFirstLoadEvent>(session);
            }
        }

        public class SessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }
    }
}

