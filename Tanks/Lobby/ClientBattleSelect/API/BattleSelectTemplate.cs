namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e67154e87L)]
    public interface BattleSelectTemplate : Template
    {
        BattleSelectComponent battleSelect();
        [AutoAdded]
        SearchResultComponent searchResult();
    }
}

