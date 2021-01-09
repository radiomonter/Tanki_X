namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientControls.API;

    public class JoinToSelectedBattleSystem : ECSSystem
    {
        [OnEventComplete]
        public void BreakJoin(NodeRemoveEvent e, SelectedBattleNode selectedBattle, [JoinByBattle, Combine] JoinedSelectedBattleNode joinedToSelectedBattle)
        {
            selectedBattle.battleGroup.Detach(joinedToSelectedBattle.Entity);
        }

        [OnEventFire]
        public void Join(NodeAddedEvent e, SelectedBattleNode selectedBattle, [Context, Combine] JoinToSelectedBattleNode joinToSelectedBattle)
        {
            selectedBattle.battleGroup.Attach(joinToSelectedBattle.Entity);
        }

        public class JoinedSelectedBattleNode : Node
        {
            public JoinToSelectedBattleComponent joinToSelectedBattle;
            public BattleGroupComponent battleGroup;
        }

        public class JoinToSelectedBattleNode : Node
        {
            public JoinToSelectedBattleComponent joinToSelectedBattle;
        }

        public class SelectedBattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
            public SelectedListItemComponent selectedListItem;
            public BattleConfiguredComponent battleConfigured;
        }
    }
}

