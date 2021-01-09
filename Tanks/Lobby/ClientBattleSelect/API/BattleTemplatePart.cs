namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientControls.API;

    [TemplatePart]
    public interface BattleTemplatePart : BattleTemplate, Template
    {
        BattleLevelRangeComponent battleLevelRange();
        BattleStartTimeComponent battleTime();
        NotFullBattleComponent notFullBattle();
        SearchDataComponent searchData();
        SelectedListItemComponent selectedListItem();
        UserCountComponent userCount();
        VisibleItemComponent visibleItem();
    }
}

