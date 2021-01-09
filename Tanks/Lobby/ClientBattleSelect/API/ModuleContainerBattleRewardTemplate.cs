namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer;

    [SerialVersionUID(0x1607cd6708dL)]
    public interface ModuleContainerBattleRewardTemplate : BattleResultRewardTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleContainerRewardTextConfigComponent moduleContainerRewardTextConfig();
    }
}

