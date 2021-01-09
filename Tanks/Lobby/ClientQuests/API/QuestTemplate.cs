namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15ba4a2e107L)]
    public interface QuestTemplate : BaseQuestTemplate, ItemImagedTemplate, Template
    {
    }
}

