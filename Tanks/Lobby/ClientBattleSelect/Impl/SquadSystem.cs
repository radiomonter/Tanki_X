namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class SquadSystem : ECSSystem
    {
        [OnEventFire]
        public void PayerInSquadAdded(NodeAddedEvent e, SquadPayerNode payer)
        {
            Debug.Log("SquadSystem.PayerInSquadAdded " + payer.Entity);
        }

        [OnEventFire]
        public void SquadAdded(NodeAddedEvent e, SquadNode squad)
        {
            Debug.Log("SquadSystem.SquadAdded " + squad.Entity);
        }

        [OnEventFire]
        public void SquadLeaderAdded(NodeAddedEvent e, [Combine] SquadLeaderNode user, [JoinBySquad] SquadNode squad)
        {
            object[] objArray1 = new object[] { "SquadSystem.SquadLeaderAdded ", user.Entity, " ", squad.Entity };
            Debug.Log(string.Concat(objArray1));
        }

        [OnEventFire]
        public void SquadLeaderRemoved(NodeRemoveEvent e, SquadLeaderNode user)
        {
            Debug.Log("SquadSystem.SquadLeaderRemoved " + user.Entity);
        }

        [OnEventFire]
        public void SquadRemoved(NodeRemoveEvent e, SquadNode squad)
        {
            Debug.Log("SquadSystem.SquadRemoved " + squad.Entity);
        }

        [OnEventFire]
        public void UserInSquadAdded(NodeAddedEvent e, [Combine] UserInSquadNode user, [JoinBySquad] SquadNode squad)
        {
            object[] objArray1 = new object[] { "SquadSystem.UserInSquadAdded ", user.Entity, " ", squad.Entity };
            Debug.Log(string.Concat(objArray1));
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
        }

        public class SquadLeaderNode : SquadSystem.UserInSquadNode
        {
            public SquadLeaderComponent squadLeader;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadConfigComponent squadConfig;
            public SquadGroupComponent squadGroup;
        }

        public class SquadPayerNode : SquadSystem.UserInSquadNode
        {
            public BattleEntrancePayerComponent battleEntrancePayer;
        }

        public class UserInSquadNode : Node
        {
            public UserComponent user;
            public SquadGroupComponent squadGroup;
        }
    }
}

