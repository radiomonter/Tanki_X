namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class LeaveSquadSystem : ECSSystem
    {
        [OnEventFire]
        public void LeaveSquad(LeaveBattleBeforeItEndEvent e, SelfUserInSquadNode user)
        {
            base.ScheduleEvent<LeaveSquadEvent>(user);
        }

        public class SelfUserInSquadNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SelfUserComponent selfUser;
            public SquadGroupComponent squadGroup;
        }
    }
}

