namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;

    public class BattleDetailsDMSystem : ECSSystem
    {
        [OnEventFire]
        public void HideDMInfoPanel(NodeRemoveEvent e, BattleDMNode battleDm, ScreenNode screen)
        {
            screen.battleSelectScreen.DMInfoPanel.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void ShowDMInfoPanel(NodeAddedEvent e, BattleDMNode battleDm, [Context, JoinByBattle] ScreenNode screen)
        {
            screen.battleSelectScreen.DMInfoPanel.gameObject.SetActive(true);
        }

        public class BattleDMNode : Node
        {
            public DMComponent dm;
            public BattleComponent battle;
            public SelectedListItemComponent selectedListItem;
            public BattleGroupComponent battleGroup;
            public UserLimitComponent userLimit;
        }

        public class ScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public BattleGroupComponent battleGroup;
        }
    }
}

