namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    internal class BattleDetailsTDMSystem : ECSSystem
    {
        [OnEventFire]
        public void HideTDMInfoPanel(NodeRemoveEvent e, BattleTDMNode battleTDM, ScreenNode screen)
        {
            screen.battleSelectScreen.TDMInfoPanel.gameObject.SetActive(false);
        }

        private void ResetVerticalScroll(RectTransform panelsContainer)
        {
            panelsContainer.anchoredPosition = new Vector2(panelsContainer.anchoredPosition.x, 0f);
        }

        [OnEventFire]
        public void ShowTDMInfoPanel(NodeAddedEvent e, BattleTDMNode battleTDM, ScreenNode screen)
        {
            RectTransform component = screen.battleSelectScreen.BattleInfoPanelsContainer.GetComponent<RectTransform>();
            this.ResetVerticalScroll(component);
            screen.battleSelectScreen.TDMInfoPanel.gameObject.SetActive(true);
        }

        public class BattleTDMNode : Node
        {
            public TeamBattleComponent teamBattle;
            public BattleComponent battle;
            public SelectedListItemComponent selectedListItem;
            public BattleGroupComponent battleGroup;
            public UserLimitComponent userLimit;
        }

        public class ScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
        }
    }
}

