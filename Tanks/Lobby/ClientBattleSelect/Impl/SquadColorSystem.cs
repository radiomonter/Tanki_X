namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class SquadColorSystem : ECSSystem
    {
        private bool ColorIsUnique(Color color, SquadsRegisterComponent squadsRegister)
        {
            <ColorIsUnique>c__AnonStorey0 storey = new <ColorIsUnique>c__AnonStorey0 {
                squadsRegister = squadsRegister,
                color = color
            };
            return storey.squadsRegister.squads.Keys.All<long>(new Func<long, bool>(storey.<>m__0));
        }

        public Color GetColorForSquad(long key, bool friendlySquad, SquadsRegisterComponent squadsRegister, SquadColorsComponent squadColors)
        {
            if (squadsRegister.squads.ContainsKey(key))
            {
                return squadsRegister.squads[key];
            }
            foreach (Color color in !friendlySquad ? squadColors.EnemyColor : squadColors.FriendlyColor)
            {
                if (this.ColorIsUnique(color, squadsRegister))
                {
                    squadsRegister.squads[key] = color;
                    return color;
                }
            }
            return Color.white;
        }

        [OnEventFire]
        public void RemoveSquadFromRegister(NodeRemoveEvent e, SquadColorLabel userLabel, [JoinBySquad] ICollection<SquadColorLabel> usersInSquad, [JoinAll] SquadRegisterNode registerNode)
        {
            userLabel.userSquadColor.Color = Color.clear;
            if ((usersInSquad.Count <= 1) && registerNode.squadsRegister.squads.ContainsKey(userLabel.squadGroup.Key))
            {
                registerNode.squadsRegister.squads.Remove(userLabel.squadGroup.Key);
            }
        }

        [OnEventFire]
        public void SetLabelColor(NodeAddedEvent e, [Combine] SquadColorLabel userLabel, [JoinAll, Context] SelfTeamUser selfUserLabel, [JoinAll] SquadRegisterNode registerNode, [JoinAll] SquadColorsNode squadColorsNode)
        {
            bool friendlySquad = userLabel.teamColor.TeamColor == selfUserLabel.teamColor.TeamColor;
            userLabel.userSquadColor.Color = this.GetColorForSquad(userLabel.squadGroup.Key, friendlySquad, registerNode.squadsRegister, squadColorsNode.squadColors);
        }

        [OnEventFire]
        public void SetSelfLabelColor(NodeAddedEvent e, SelfSquadUser selfUserLabel, [JoinAll] SquadRegisterNode registerNode, [JoinAll] SquadColorsNode squadColorsNode)
        {
            registerNode.squadsRegister.squads[selfUserLabel.squadGroup.Key] = squadColorsNode.squadColors.SelfSquadColor;
        }

        [CompilerGenerated]
        private sealed class <ColorIsUnique>c__AnonStorey0
        {
            internal SquadsRegisterComponent squadsRegister;
            internal Color color;

            internal bool <>m__0(long key) => 
                !this.squadsRegister.squads[key].Equals(this.color);
        }

        public class SelfSquadUser : SquadColorSystem.SelfTeamUser
        {
            public SquadGroupComponent squadGroup;
        }

        public class SelfTeamUser : Node
        {
            public UserGroupComponent userGroup;
            public TeamColorComponent teamColor;
            public SelfUserComponent selfUser;
        }

        public class SquadColorLabel : Node
        {
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
            public UserSquadColorComponent userSquadColor;
            public TeamColorComponent teamColor;
        }

        public class SquadColorsNode : Node
        {
            public SquadColorsComponent squadColors;
        }

        public class SquadRegisterNode : Node
        {
            public SquadsRegisterComponent squadsRegister;
        }
    }
}

