namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientEntrance.Impl;

    [SerialVersionUID(0x14e24b80861L)]
    public interface AuthentificationTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        EntranceValidationRulesComponent entranceValidationRules();
    }
}

