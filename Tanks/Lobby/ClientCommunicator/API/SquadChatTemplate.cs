namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientCommunicator.Impl;

    [SerialVersionUID(0x8d53b1076f7a365L)]
    public interface SquadChatTemplate : Template
    {
        ChatParticipantsComponent chatParticipants();
        [AutoAdded]
        SquadChatComponent squadChat();
    }
}

