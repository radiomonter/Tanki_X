namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class SquadEnergySystem : ECSSystem
    {
        [OnEventFire]
        public void CheckSquadEnergy(CheckSquadEnergyEvent e, SquadSelfUserNode squadSelfUser, [JoinBySquad] ICollection<SquadUserNode> squadUsers)
        {
            bool flag = true;
            foreach (SquadUserNode node in squadUsers)
            {
                CheckUserEnergyEvent eventInstance = new CheckUserEnergyEvent();
                base.ScheduleEvent(eventInstance, node);
                flag &= eventInstance.HaveEnoughtEnergyForEntrance;
                object[] objArray1 = new object[] { "SquadEnergySystem.CheckSquadEnergy ", node.Entity, " ", eventInstance.HaveEnoughtEnergyForEntrance, " ", flag };
                Debug.Log(string.Concat(objArray1));
            }
            e.HaveEnoughtEnergyForEntrance = flag;
        }

        [OnEventFire]
        public void CheckUserEnergy(CheckUserEnergyEvent e, SquadUserNode user, [JoinByUser] EnergyUserItemNode energy, SquadUserNode userToLeague, [JoinByLeague] SingleNode<LeagueEnergyConfigComponent> league)
        {
            e.HaveEnoughtEnergyForEntrance = energy.userItemCounter.Count >= league.component.Cost;
        }

        public class CheckUserEnergyEvent : Event
        {
            public bool HaveEnoughtEnergyForEntrance { get; set; }
        }

        public class EnergyUserItemNode : Node
        {
            public UserGroupComponent userGroup;
            public EnergyItemComponent energyItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadGroupComponent squadGroup;
        }

        public class SquadSelfUserNode : SquadEnergySystem.SquadUserNode
        {
            public SelfUserComponent selfUser;
        }

        public class SquadUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
        }
    }
}

