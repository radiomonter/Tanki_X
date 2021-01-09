namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class TesterUserSystem : ElevatedAccessUserBaseSystem
    {
        private void InitSquadTester(string parameters)
        {
            base.ScheduleEvent(new InitSquadTesterEvent(), base.user);
        }

        [OnEventFire]
        public void InitTesterConsole(NodeAddedEvent e, TesterUserNode tester)
        {
        }

        public class TesterUserNode : ElevatedAccessUserBaseSystem.SelfUserNode
        {
            public UserTesterComponent userTester;
        }
    }
}

