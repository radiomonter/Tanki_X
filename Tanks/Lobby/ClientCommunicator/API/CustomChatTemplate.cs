namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientCommunicator.Impl;

    [SerialVersionUID(0x8d521531052197dL)]
    public interface CustomChatTemplate : Template
    {
        ChatParticipantsComponent chatParticipants();
        [AutoAdded]
        CustomChatComponent customChat();
    }
}

