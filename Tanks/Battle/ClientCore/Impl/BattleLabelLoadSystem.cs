namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class BattleLabelLoadSystem : ECSSystem
    {
        [OnEventFire]
        public void LoadedUser(BattleInfoForLabelLoadedEvent e, SelfUserNode selfUser, [JoinAll] ICollection<EmptyBattleLabelNode> battleLabels)
        {
            foreach (EmptyBattleLabelNode node in battleLabels)
            {
                long battleId = node.battleLabel.BattleId;
                if (battleId.Equals(e.BattleId))
                {
                    BattleInfoForLabelComponent component = new BattleInfoForLabelComponent {
                        BattleMode = e.BattleMode
                    };
                    node.Entity.AddComponent(component);
                    e.Map.GetComponent<MapGroupComponent>().Attach(node.Entity);
                }
            }
        }

        [OnEventFire]
        public void LoadedUser(BattleInfoForLabelLoadedEvent e, SelfUserNode selfUser, [JoinAll] ICollection<UserLabelStateNode> userLabels)
        {
            foreach (UserLabelStateNode node in userLabels)
            {
                long key = node.battleGroup.Key;
                if (key.Equals(e.BattleId))
                {
                    string battleMode = e.BattleMode;
                    node.userLabelState.SetBattleDescription(battleMode, e.Map.GetComponent<DescriptionItemComponent>().Name);
                }
            }
        }

        [OnEventFire]
        public void OnEnabledBattleLabel(NodeAddedEvent e, BattleLabelReadyNode battleLabel, [JoinAll] SelfUserNode selfUser)
        {
            long battleId = battleLabel.battleLabel.BattleId;
            base.ScheduleEvent(new RequestLoadBattleInfoEvent(battleId), selfUser);
        }

        [OnEventFire]
        public void OnEnabledUserLabelState(NodeAddedEvent e, UserLabelStateNode userLabel, [JoinAll] SelfUserNode selfUser)
        {
            long key = userLabel.battleGroup.Key;
            base.ScheduleEvent(new RequestLoadBattleInfoEvent(key), selfUser);
        }

        public class BattleLabelReadyNode : Node
        {
            public BattleLabelComponent battleLabel;
            public BattleLabelReadyComponent battleLabelReady;
        }

        [Not(typeof(BattleInfoForLabelComponent))]
        public class EmptyBattleLabelNode : BattleLabelLoadSystem.BattleLabelReadyNode
        {
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        public class UserLabelStateNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserLabelStateComponent userLabelState;
        }
    }
}

