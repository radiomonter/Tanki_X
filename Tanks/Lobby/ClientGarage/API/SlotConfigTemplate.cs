namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4a6a61634c793L)]
    public interface SlotConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("slotsTypes", false)]
        Slots2ModuleBehaviourTypesConfigComponent slots2ModuleBehaviourTypesConfig();
    }
}

